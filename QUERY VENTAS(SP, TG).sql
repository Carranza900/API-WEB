--PROCEDIMIENTO ALMACENADO PARA CREAR LA VENTA

CREATE PROCEDURE [dbo].[sp_CrearVenta]
    @IdCliente int,
    @IdUsuario int,
	@NumFac varchar(15),
	@Fecha datetime,
	@Subtotal decimal(18,2),
	@Iva decimal (18,2),
	@Total decimal(18,2),
    @Detalles TDetalleVenta READONLY 
AS
BEGIN
    SET NOCOUNT ON;
	--VALIDACION SI EXISTE EL CLIENTE EN LA BD
	 IF NOT EXISTS (SELECT 1 FROM Clientes WHERE Id_Cliente = @IdCliente)
    BEGIN
        THROW 50000, 'Error: El IdCliente no existe en la tabla Clientes.', 1;
    END

	--VALIDACION SI EXISTE EL USUARIO EN LA BD
 IF NOT EXISTS (SELECT 1 FROM Usuarios WHERE Id_Usuario = @IdUsuario)
    BEGIN
        THROW 50001, 'Error: El IdUsuario no existe en la tabla Usuarios.', 1;
    END

  --VALIDACION SI EXISTE EL PRODUCTO EN LA BD
    IF EXISTS (
        SELECT 1
        FROM @Detalles d
        LEFT JOIN Productos p ON d.Id_Producto = p.ID_Producto
        WHERE p.Id_Producto IS NULL
    )
    BEGIN
        THROW 50002, 'Error: Uno o más Id_Producto en los detalles no existen en la tabla Productos.', 1;
    END

    BEGIN TRY
        BEGIN TRANSACTION;
		DECLARE @IdVenta int;  

        INSERT INTO Ventas(Id_Cliente,Id_Usuario,Num_Factura,FechaVenta,subtotal,IVA,Total)
        VALUES (@IdCliente, @IdUsuario, @NumFac,@Fecha,@Subtotal, @Iva, @Total);

        SET @IdVenta= SCOPE_IDENTITY();

         INSERT INTO DetalleVenta(Id_Venta, Id_Producto, Precio,Cantidad, Subtotal)
        SELECT @IdVenta, Id_Producto,Precio, Cantidad, Subtotal
        FROM @Detalles;

        COMMIT TRANSACTION;
		  SELECT 'Venta registrada exitosamente' AS Mensaje, @IdVenta AS IdVentaCreada;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;


--EJECUTAR EL PROCEDIMIENTO ALMACENADO
DECLARE @TablaDetalles TDetalleVenta;

INSERT INTO @TablaDetalles (Id_Producto, Precio, Cantidad, Subtotal)
VALUES (3, 10.00, 2, 20.00)

EXEC sp_CrearVenta 
    @IdCliente = 1,
    @IdUsuario = 2,
    @NumFac = 'F12345',
    @Fecha = '2023-09-12',
    @Subtotal = 20.00,
    @Iva = 3,
    @Total = 23.00,
    @Detalles = @TablaDetalles;

	--CONSULTAS PARA VERIFICAR LOS DATOS
	SELECT * FROM Ventas
	SELECT * FROM DetalleVenta

--TRIGGER PARA EL MANEJO DE LAS EXISTENCIAS EN LA TABLA DETALLE PRODUCTO ALMACENADO
CREATE TRIGGER Tg_AjustesExistenciasVenta
ON DetalleVenta
AFTER INSERT
AS
BEGIN
 DECLARE @IdProducto INT, @Cantidad INT;
	IF EXISTS (
    SELECT 1
    FROM inserted i
    JOIN DetalleProductoAlmacen DPA ON i.Id_Producto= DPA.Id_Producto
	where i.Cantidad > dpa.Existencia)

BEGIN
    ROLLBACK TRANSACTION;
	THROW 50003, 'Error: Existencias insuficientes .', 1;
END
    UPDATE DPA
    SET DPA.Existencia = DPA.Existencia - i.Cantidad
    FROM inserted i
JOIN DetalleProductoAlmacen DPA ON i.Id_Producto= DPA.Id_Producto
END;

--PROCEDIMIENTO PARA LISTAR LAS VENTAS BY ID
CREATE PROCEDURE sp_ListarVentasId
 @IdVenta INT
AS
BEGIN

    SELECT 
        v.ID_Venta,
        v.Num_Factura,
        c.Nombre AS Cliente,
        u.UsuarioName AS Usuario,
        v.Subtotal,
        v.IVA,
        v.Total,
        v.FechaVenta
    FROM 
        Ventas v
    JOIN 
        Clientes c ON v.Id_Cliente = c.ID_Cliente
    JOIN 
        Usuarios u ON v.Id_Usuario = u.ID_Usuario
		where v.ID_Venta= @IdVenta

    SELECT 
        dv.Id_Venta,
        p.Nombre AS Producto,
        dv.Cantidad,
        dv.Subtotal AS DetalleSubtotal
    FROM 
        DetalleVenta dv
    JOIN 
        Productos p ON dv.Id_Producto = p.ID_Producto
		WHERE 
        dv.Id_Venta = @IdVenta; 
END

--PROCEDIMIENTO PARA LISTAR LAS VENTAS
CREATE PROCEDURE sp_ListarVentas
AS
BEGIN

    SELECT 
        v.ID_Venta,
        v.Num_Factura,
        c.Nombre AS Cliente,
        u.UsuarioName AS Usuario,
        v.Subtotal,
        v.IVA,
        v.Total,
        v.FechaVenta
    FROM 
        Ventas v
    JOIN 
        Clientes c ON v.Id_Cliente = c.ID_Cliente
    JOIN 
        Usuarios u ON v.Id_Usuario = u.ID_Usuario
    ORDER BY v.ID_Venta;

    SELECT 
        dv.Id_Venta,
        p.Nombre AS Producto,
        dv.Cantidad,
        dv.Subtotal AS DetalleSubtotal
    FROM 
        DetalleVenta dv
    JOIN 
        Productos p ON dv.Id_Producto = p.ID_Producto
END

--procedimiento para actualizar la venta
CREATE PROCEDURE sp_ActualizarVenta
    @IdVenta INT,
    @IdCliente INT,
    @IdUsuario INT,
    @NumFac VARCHAR(15),
    @Fecha DATETIME,
    @Subtotal DECIMAL(18, 2),
    @IVA DECIMAL(18, 2),
    @Total DECIMAL(18, 2),
    @Detalles TDetalleVenta READONLY
AS
BEGIN
    SET NOCOUNT ON;

  IF NOT EXISTS (SELECT 1 FROM Ventas WHERE ID_Venta = @IdVenta)
    BEGIN
        THROW 50000, 'Error: La venta no existe.', 1;
    END

    
    IF NOT EXISTS (SELECT 1 FROM Clientes WHERE ID_Cliente = @IdCliente)
    BEGIN
        THROW 50001, 'Error: El cliente no existe.', 1;
    END

    -- Verificar si el usuario existe
    IF NOT EXISTS (SELECT 1 FROM Usuarios WHERE ID_Usuario = @IdUsuario)
    BEGIN
        THROW 50002, 'Error: El usuario no existe.', 1;
    END

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Actualizar la venta
        UPDATE Ventas
        SET Id_Cliente = @IdCliente,
            Id_Usuario = @IdUsuario,
            Num_Factura = @NumFac,
            FechaVenta = @Fecha,
            Subtotal = @Subtotal,
            IVA = @IVA,
            Total = @Total
        WHERE ID_Venta = @IdVenta;

		  DELETE FROM DetalleVenta WHERE Id_Venta = @IdVenta;

		   INSERT INTO DetalleVenta (Id_Venta, Id_Producto, Precio, Cantidad, Subtotal)
        SELECT @IdVenta, Id_Producto, Precio, Cantidad, Subtotal FROM @Detalles;

        COMMIT TRANSACTION;
		SELECT 'Venta y detalles actualizados correctamente' AS Mensaje;
           END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
