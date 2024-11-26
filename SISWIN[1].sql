-- Crear la base de datos
CREATE DATABASE SISWIN;
USE SISWIN;


CREATE TABLE Bitacora 
( 
   Codigo int IDENTITY PRIMARY KEY, 
   CodigoEntidad int, 
   Nombre nvarchar(50), 
   Evento nvarchar(15),
   Fecha datetime,
   UsuarioName nvarchar(30), 
   Aplicacion nvarchar(150),
   Terminal nvarchar(150),
   ValorEstimado decimal(18, 2) NULL,
   ValorAnterior nvarchar(200) NULL,
   ValorNuevo nvarchar(200) NULL
);

CREATE TRIGGER trg_Insert_Bitacora
ON Bitacora
AFTER INSERT
AS
BEGIN
    DECLARE @UsuarioName nvarchar(50);

    -- Obtener el nombre del usuario que insert� (suponiendo que se guarda en `UsuarioName`)
    SELECT @UsuarioName = UsuarioName FROM INSERTED;

    -- Insertar el registro de auditor�a para el evento de inserci�n
    INSERT INTO Auditoria (NombreTabla, TipoOperacion, Usuario, ValorNuevo)
    SELECT 'Bitacora', 'INSERT', @UsuarioName, 
           CONCAT('Codigo: ', Codigo, ', CodigoEntidad: ', CodigoEntidad, 
                  ', Nombre: ', Nombre, ', Evento: ', Evento, 
                  ', Fecha: ', Fecha, ', Aplicacion: ', Aplicacion,
                  ', Terminal: ', Terminal, ', ValorEstimado: ', ValorEstimado,
                  ', ValorNuevo: ', ValorNuevo)
    FROM INSERTED;
END;

CREATE TRIGGER trg_Update_Bitacora
ON Bitacora
AFTER UPDATE
AS
BEGIN
    DECLARE @UsuarioName nvarchar(50);

    -- Obtener el nombre del usuario que actualiz�
    SELECT @UsuarioName = UsuarioName FROM INSERTED;

    -- Insertar el registro de auditor�a para el evento de actualizaci�n
    INSERT INTO Auditoria (NombreTabla, TipoOperacion, Usuario, ValorAnterior, ValorNuevo)
    SELECT 'Bitacora', 'UPDATE', @UsuarioName,
           CONCAT('Codigo: ', DELETED.Codigo, ', CodigoEntidad: ', DELETED.CodigoEntidad, 
                  ', Nombre: ', DELETED.Nombre, ', Evento: ', DELETED.Evento, 
                  ', Fecha: ', DELETED.Fecha, ', Aplicacion: ', DELETED.Aplicacion,
                  ', Terminal: ', DELETED.Terminal, ', ValorEstimado: ', DELETED.ValorEstimado,
                  ', ValorAnterior: ', DELETED.ValorAnterior),
           CONCAT('Codigo: ', INSERTED.Codigo, ', CodigoEntidad: ', INSERTED.CodigoEntidad, 
                  ', Nombre: ', INSERTED.Nombre, ', Evento: ', INSERTED.Evento, 
                  ', Fecha: ', INSERTED.Fecha, ', Aplicacion: ', INSERTED.Aplicacion,
                  ', Terminal: ', INSERTED.Terminal, ', ValorEstimado: ', INSERTED.ValorEstimado,
                  ', ValorNuevo: ', INSERTED.ValorNuevo)
    FROM INSERTED
    JOIN DELETED ON INSERTED.Codigo = DELETED.Codigo;
END;

CREATE TRIGGER trg_Delete_Bitacora
ON Bitacora
AFTER DELETE
AS
BEGIN
    DECLARE @UsuarioName nvarchar(50);

    -- Obtener el nombre del usuario que fue eliminado
    SELECT @UsuarioName = UsuarioName FROM DELETED;

    -- Insertar el registro de auditor�a para el evento de eliminaci�n
    INSERT INTO Auditoria (NombreTabla, TipoOperacion, Usuario, ValorAnterior)
    SELECT 'Bitacora', 'DELETE', @UsuarioName,
           CONCAT('Codigo: ', Codigo, ', CodigoEntidad: ', CodigoEntidad, 
                  ', Nombre: ', Nombre, ', Evento: ', Evento, 
                  ', Fecha: ', Fecha, ', Aplicacion: ', Aplicacion,
                  ', Terminal: ', Terminal, ', ValorEstimado: ', ValorEstimado,
                  ', ValorAnterior: ', ValorAnterior)
    FROM DELETED;
END;


CREATE TABLE Auditoria
(
    ID_Auditoria int PRIMARY KEY IDENTITY(1,1),
    NombreTabla nvarchar(50) NOT NULL,         -- Nombre de la tabla modificada
    TipoOperacion nvarchar(10) NOT NULL,       -- Tipo de operaci�n (INSERT, UPDATE, DELETE)
    FechaOperacion datetime NOT NULL DEFAULT GETDATE(), -- Fecha y hora de la operaci�n
    Usuario nvarchar(50) NOT NULL,             -- Usuario que realiz� la operaci�n
    ValorAnterior nvarchar(max) NULL,          -- Valor antes de la modificaci�n
    ValorNuevo nvarchar(max) NULL              -- Valor despu�s de la modificaci�n
);



CREATE TRIGGER trg_Insert_Usuarios
ON Usuarios
AFTER INSERT
AS
BEGIN
    DECLARE @UsuarioName nvarchar(50);

    -- Obtener el nombre del usuario que insert�
    SELECT @UsuarioName = UsuarioName FROM INSERTED;

    -- Insertar el registro de auditor�a
    INSERT INTO Auditoria (NombreTabla, TipoOperacion, Usuario, ValorNuevo)
    SELECT 'Usuarios', 'INSERT', @UsuarioName, CONCAT('ID_Usuario: ', ID_Usuario, ', UsuarioName: ', UsuarioName, ', Rol: ', Id_Rol, ', Estado: ', Estado)
    FROM INSERTED;
END;

CREATE TRIGGER trg_Update_Usuarios
ON Usuarios
AFTER UPDATE
AS
BEGIN
    DECLARE @UsuarioName nvarchar(50);

    -- Obtener el nombre del usuario que actualiz�
    SELECT @UsuarioName = UsuarioName FROM INSERTED;

    -- Insertar el registro de auditor�a
    INSERT INTO Auditoria (NombreTabla, TipoOperacion, Usuario, ValorAnterior, ValorNuevo)
    SELECT 'Usuarios', 'UPDATE', @UsuarioName,
           CONCAT('ID_Usuario: ', DELETED.ID_Usuario, ', UsuarioName: ', DELETED.UsuarioName, ', Rol: ', DELETED.Id_Rol, ', Estado: ', DELETED.Estado),
           CONCAT('ID_Usuario: ', INSERTED.ID_Usuario, ', UsuarioName: ', INSERTED.UsuarioName, ', Rol: ', INSERTED.Id_Rol, ', Estado: ', INSERTED.Estado)
    FROM INSERTED
    JOIN DELETED ON INSERTED.ID_Usuario = DELETED.ID_Usuario;
END;

CREATE TRIGGER trg_Delete_Usuarios
ON Usuarios
AFTER DELETE
AS
BEGIN
    DECLARE @UsuarioName nvarchar(50);

    -- Obtener el nombre del usuario que fue eliminado
    SELECT @UsuarioName = UsuarioName FROM DELETED;

    -- Insertar el registro de auditor�a
    INSERT INTO Auditoria (NombreTabla, TipoOperacion, Usuario, ValorAnterior)
    SELECT 'Usuarios', 'DELETE', @UsuarioName,
           CONCAT('ID_Usuario: ', ID_Usuario, ', UsuarioName: ', UsuarioName, ', Rol: ', Id_Rol, ', Estado: ', Estado)
    FROM DELETED;
END;


-- Tabla de Roles
CREATE TABLE Roles
(
    ID_Rol int IDENTITY(1,1) PRIMARY KEY,
    NombreRol varchar(40)
);

-- Tabla de Usuarios
CREATE TABLE Usuarios
(
    ID_Usuario int PRIMARY KEY IDENTITY(1,1),
    UsuarioName varchar(15),
    UsuarioClave varchar(20),
    Id_Rol int,
    Estado bit,
    CONSTRAINT FK_Roles FOREIGN KEY (Id_Rol) REFERENCES Roles(ID_Rol)
);

-- Tabla de Clientes
CREATE TABLE Clientes
(
    ID_Cliente int PRIMARY KEY IDENTITY(1,1),
    Nombre varchar(40),
    Apellido varchar(40),
    Telefono varchar(15)
);

-- Tabla de Proveedores
CREATE TABLE Proveedores
(
    ID_Proveedor int PRIMARY KEY IDENTITY(1,1),
    NumRuc varchar(11),
    Razon_Social varchar(20),
    NombreProveedor varchar(20),
    Apellido varchar(20),
    Telefono varchar(15),
    Direccion varchar(30),
    Correo varchar(40)
);

-- Tabla de Categor�as
CREATE TABLE Categorias
(
    ID_Categoria int PRIMARY KEY IDENTITY(1,1),
    Nombre varchar(50),
    Descripcion varchar(60),
    Estado bit
);

-- Tabla de Productos
CREATE TABLE Productos
(
    ID_Producto int PRIMARY KEY IDENTITY(1,1),
    Id_Categoria int,
    Nombre varchar(40),
    Descripcion varchar(50),
    Estado bit,
    CONSTRAINT FK_Categoria FOREIGN KEY (Id_Categoria) REFERENCES Categorias(ID_Categoria)
);
select *from Producto_Caracteristica
-- Tabla de Caracteristicas
CREATE TABLE Caracteristicas
(
    ID_Caracteristica int PRIMARY KEY IDENTITY(1,1),
    Nombre varchar(50) NOT NULL,
    Descripcion varchar(100),
    ValorEstimado decimal(18,2) -- Este campo representar� el valor estimado
);

-- Tabla intermedia para la relaci�n muchos a muchos entre Productos y Caracteristicas
CREATE TABLE Producto_Caracteristica
(
    ID_Producto int,
    ID_Caracteristica int,
    CONSTRAINT FK_ProductoCaracteristica_Producto FOREIGN KEY (ID_Producto) REFERENCES Productos(ID_Producto),
    CONSTRAINT FK_ProductoCaracteristica_Caracteristica FOREIGN KEY (ID_Caracteristica) REFERENCES Caracteristicas(ID_Caracteristica),
    PRIMARY KEY (ID_Producto, ID_Caracteristica) 
);
SELECT *FROM Ubicaciones
-- Tabla de Almac�n (ubicaciones f�sicas)
CREATE TABLE Almacen
(
    ID_Almacen int PRIMARY KEY IDENTITY(1,1),
    Nombre varchar(40),
    Descripcion varchar(50)
);
Select *from Ubicaciones
-- Tabla de Ubicaciones (ubicaciones virtuales dentro de un almac�n)
CREATE TABLE Ubicaciones
(
    ID_Ubicacion int PRIMARY KEY IDENTITY(1,1),
    ID_Almacen int,
    Nombre varchar(40),
    Descripcion varchar(50),
    CONSTRAINT FK_Almacen FOREIGN KEY (ID_Almacen) REFERENCES Almacen(ID_Almacen)
);

-- Tabla de Lote
CREATE TABLE Lote
(
    ID_Lote int PRIMARY KEY IDENTITY(1,1),
    Codigo varchar(20),
    Fecha_Entrada datetime,
    Fecha_Vencimiento datetime
);

-- Tabla de DetalleProductoAlmacen
CREATE TABLE DetalleProductoAlmacen
(
    ID_DetalleProducto int PRIMARY KEY IDENTITY(1,1),
    Id_Producto int,
    Id_Ubicacion int,
    Id_Proveedor int,
    Id_Lote int,
    Existencia int,
    Precio_Compra decimal(18,2),
    Precio_Venta decimal(18,2),
    CONSTRAINT FK_Producto FOREIGN KEY (Id_Producto) REFERENCES Productos(ID_Producto),
    CONSTRAINT FK_Ubicacion FOREIGN KEY (Id_Ubicacion) REFERENCES Ubicaciones(ID_Ubicacion),
    CONSTRAINT FK_Proveedor FOREIGN KEY (Id_Proveedor) REFERENCES Proveedores(ID_Proveedor),
    CONSTRAINT FK_Lote FOREIGN KEY (Id_Lote) REFERENCES Lote(ID_Lote)
);

-- Tabla de Ventas
CREATE TABLE Ventas 
(
    ID_Venta int PRIMARY KEY IDENTITY(1,1),
    Id_Cliente int,
    Id_Usuario int,
    Num_Factura varchar(15),
    FechaVenta datetime,
    Subtotal decimal(18,2),
    IVA decimal(18,2),
    Total decimal(18,2),
    CONSTRAINT FK_Cliente FOREIGN KEY (Id_Cliente) REFERENCES Clientes(ID_Cliente),
    CONSTRAINT FK_Usuario FOREIGN KEY (Id_Usuario) REFERENCES Usuarios(ID_Usuario)
);


CREATE TABLE DetalleVenta
(
    ID_Detalle_Venta int PRIMARY KEY IDENTITY(1,1),
    Id_Venta int,
    Id_DetalleProducto int,
    Precio decimal(18,2),
    Cantidad int,
    Subtotal decimal(18,2),
    FOREIGN KEY (Id_DetalleProducto) REFERENCES DetalleProductoAlmacen(ID_DetalleProducto),
    FOREIGN KEY (Id_Venta) REFERENCES Ventas(ID_Venta)
);

-- Tabla de Compras
CREATE TABLE Compras
(
    ID_Compra int PRIMARY KEY IDENTITY(1,1),
    Id_Proveedor int,
    Id_Usuario int,
    Num_Factura varchar(15),
    FechaCompra datetime,
    Subtotal decimal(18,2),
    IVA decimal(18,2),
    Total decimal(18,2),
    FOREIGN KEY (Id_Proveedor) REFERENCES Proveedores(ID_Proveedor),
    FOREIGN KEY (Id_Usuario) REFERENCES Usuarios(ID_Usuario)
);


CREATE TABLE DetalleCompra
(
    ID_Detalle_Compra int PRIMARY KEY IDENTITY(1,1),
    Id_Compra int,
	NombreProveedor int,
	NombreUsuario int ,
    Id_DetalleProducto int,
    Precio_Compra decimal(18,2),
    Cantidad int,
    Subtotal decimal(18,2),
	Total decimal (18,2)
    FOREIGN KEY (Id_DetalleProducto) REFERENCES DetalleProductoAlmacen(ID_DetalleProducto),
    FOREIGN KEY (Id_Compra) REFERENCES Compras(ID_Compra)
);


CREATE TABLE Devoluciones
(
    ID_Devolucion int PRIMARY KEY IDENTITY(1,1),
    Id_Usuario int,
    Id_Venta int,
    Id_DetalleProducto int,
    EstadoProducto varchar(50),
    Cantidad int,
    Motivo varchar(100),
    Fecha_Devolucion datetime,
    CONSTRAINT FK_Devoluciones_Venta FOREIGN KEY (Id_Venta) REFERENCES Ventas(ID_Venta),
    CONSTRAINT FK_Devoluciones_DetalleProducto FOREIGN KEY (Id_DetalleProducto) REFERENCES DetalleProductoAlmacen(ID_DetalleProducto),
    CONSTRAINT FK_Devoluciones_Usuario FOREIGN KEY (Id_Usuario) REFERENCES Usuarios(ID_Usuario)
);


CREATE PROCEDURE ListarCategorias
AS
BEGIN
    SELECT 
        ID_Categoria, 
        Nombre, 
        Descripcion, 
        CASE 
            WHEN Estado = 1 THEN 'Activo' 
            ELSE 'Inactivo' 
        END AS Estado
    FROM 
        Categorias;
END;
GO

CREATE PROCEDURE InsertarCategoria
    @Nombre varchar(50),
    @Descripcion nvarchar(60),
    @Estado bit
AS
BEGIN
    INSERT INTO Categorias (Nombre, Descripcion, Estado)
    VALUES (@Nombre, @Descripcion, @Estado);
END;
GO

CREATE PROCEDURE ActualizarCategoria
    @ID_Categoria int,
    @Nombre varchar(50),
    @Descripcion nvarchar(60),
    @Estado bit
AS
BEGIN
    UPDATE Categorias
    SET 
        Nombre = @Nombre,
        Descripcion = @Descripcion,
        Estado = @Estado
    WHERE ID_Categoria = @ID_Categoria;
END;
GO

CREATE PROCEDURE EliminarCategoria
    @ID_Categoria int
AS
BEGIN
    DELETE FROM Categorias
    WHERE ID_Categoria = @ID_Categoria;
END;
GO

CREATE PROCEDURE ObtenerCategoriaPorId
    @ID_Categoria int
AS
BEGIN
    SELECT 
        ID_Categoria, 
        Nombre, 
        Descripcion, 
        CASE 
            WHEN Estado = 1 THEN 'Activo' 
            ELSE 'Inactivo' 
        END AS Estado
    FROM 
        Categorias
    WHERE 
        ID_Categoria = @ID_Categoria;
END;
GO

INSERT INTO Proveedores (NumRuc, Razon_Social, Nombre, Apellido, Telefono, Direccion, Correo)
VALUES
('12345678901', 'Proveedor ABC S.A.', 'Juan', 'P�rez', '987654321', 'Calle 123, Managua', 'juan.perez@abc.com');

INSERT INTO Ubicaciones (Nombre) VALUES ('Ubicaci�n A');

INSERT INTO Lote (Codigo, Fecha_Entrada, Fecha_Vencimiento)
VALUES ('L001', '2024-11-01', '2025-11-01');


select *from Proveedores

EXEC ListarCategorias;
EXEC InsertarCategoria @Nombre = 'Helados de Frutas', @Descripcion = 'Helados elaborados con diferentes tipos de frutas.', @Estado = 1;
EXEC ObtenerCategoriaPorId @ID_Categoria = 1;




Select *From Lote
-- Definir el tipo de tabla para las caracter�sticas
CREATE TYPE CaracteristicasType AS TABLE
(
    Descripcion VARCHAR(100)
);


CREATE TYPE DetalleProductoAlmacenType AS TABLE
(
    Id_Ubicacion INT,
    Id_Proveedor INT,
    Id_Lote INT,
    Existencia INT,
    Precio_Compra DECIMAL(18,2),
    Precio_Venta DECIMAL(18,2)
);


CREATE PROCEDURE InsertarProducto
    @Id_Categoria INT,
    @Nombre VARCHAR(40),
    @Descripcion VARCHAR(50),
    @Estado BIT,
    @Detalles DetalleProductoAlmacenType READONLY,  -- Usar el tipo de tabla para los detalles
    @Caracteristicas CaracteristicasType READONLY   -- Usar el tipo de tabla para las caracter�sticas
AS
BEGIN
    BEGIN TRANSACTION;
    BEGIN TRY
        -- Validar que todos los proveedores existan
        DECLARE @ProveedorExistente INT;
        SELECT @ProveedorExistente = COUNT(1)
        FROM Proveedores
        WHERE ID_Proveedor IN (SELECT Id_Proveedor FROM @Detalles);

        IF @ProveedorExistente <> (SELECT COUNT(*) FROM @Detalles)
        BEGIN
            THROW 50000, 'Error: El proveedor no existe.', 1;
        END

        -- Validar que todas las ubicaciones existan
        DECLARE @UbicacionExistente INT;
        SELECT @UbicacionExistente = COUNT(1)
        FROM Ubicaciones
        WHERE ID_Ubicacion IN (SELECT Id_Ubicacion FROM @Detalles);

        IF @UbicacionExistente <> (SELECT COUNT(*) FROM @Detalles)
        BEGIN
            THROW 50000, 'Error: La ubicaci�n no existe.', 1;
        END

        -- Validar que todos los lotes existan
        DECLARE @LoteExistente INT;
        SELECT @LoteExistente = COUNT(1)
        FROM Lote
        WHERE ID_Lote IN (SELECT Id_Lote FROM @Detalles);

        IF @LoteExistente <> (SELECT COUNT(*) FROM @Detalles)
        BEGIN
            THROW 50000, 'Error: El lote no existe.', 1;
        END

        -- Validar que todas las caracter�sticas existan
        DECLARE @CaracteristicaExistente INT;
        SELECT @CaracteristicaExistente = COUNT(1)
        FROM Caracteristicas
        WHERE Descripcion IN (SELECT Descripcion FROM @Caracteristicas);

        IF @CaracteristicaExistente <> (SELECT COUNT(*) FROM @Caracteristicas)
        BEGIN
            THROW 50000, 'Error: La caracter�stica no existe.', 1;
        END

        -- Insertar el Producto (tabla principal)
        INSERT INTO Productos (Id_Categoria, Nombre, Descripcion, Estado)
        VALUES (@Id_Categoria, @Nombre, @Descripcion, @Estado);

        -- Obtener el ID del nuevo Producto
        DECLARE @NuevoIDProducto INT;
        SET @NuevoIDProducto = SCOPE_IDENTITY();

        -- Insertar los detalles en DetalleProductoAlmacen
        INSERT INTO DetalleProductoAlmacen (Id_Producto, Id_Ubicacion, Id_Proveedor, Id_Lote, Existencia, Precio_Compra, Precio_Venta)
        SELECT @NuevoIDProducto, 
               dp.Id_Ubicacion, 
               dp.Id_Proveedor, 
               dp.Id_Lote, 
               dp.Existencia, 
               dp.Precio_Compra, 
               dp.Precio_Venta
        FROM @Detalles dp;

        -- Insertar las relaciones de Producto con Caracter�sticas
        INSERT INTO ProductoCaracteristica (Id_Producto, Id_Caracteristica)
        SELECT @NuevoIDProducto, c.Id_Caracteristica
        FROM @Caracteristicas c
        WHERE EXISTS (SELECT 1 FROM Caracteristicas ca WHERE ca.Descripcion = c.Descripcion);

        -- Confirmar la transacci�n
        COMMIT;
    END TRY
    BEGIN CATCH
        -- Si ocurre un error, deshacer la transacci�n
        ROLLBACK;
        THROW; -- Propaga el error
    END CATCH
END;




CREATE PROCEDURE ListarProductos
AS
BEGIN
    -- Seleccionamos los productos con sus detalles
    SELECT 
        p.ID_Producto,
        p.Nombre AS Producto,
        p.Descripcion,
        p.Estado AS EstadoProducto,
        c.Nombre AS Categoria,
        ua.Nombre AS Ubicacion,
        pr.Nombre AS Proveedor,
        l.Codigo AS Lote,
        dpa.Existencia,
        dpa.Precio_Compra,
        dpa.Precio_Venta
    FROM 
        Productos p
    INNER JOIN 
        Categorias c ON p.Id_Categoria = c.ID_Categoria
    INNER JOIN 
        DetalleProductoAlmacen dpa ON p.ID_Producto = dpa.Id_Producto
    INNER JOIN 
        Ubicaciones ua ON dpa.Id_Ubicacion = ua.ID_Ubicacion
    INNER JOIN 
        Proveedores pr ON dpa.Id_Proveedor = pr.ID_Proveedor
    INNER JOIN 
        Lote l ON dpa.Id_Lote = l.ID_Lote
    ORDER BY 
        p.ID_Producto; -- Puedes cambiar el orden seg�n lo que necesites
END;

exec ListarProductos
SELECT * FROM 

CREATE PROCEDURE ObtenerProductosPorAlmacen
    @IdAlmacen INT 
AS
BEGIN
    SELECT p.ID_Producto, p.Nombre, p.Descripcion, dp.Existencia, dp.Precio_Compra, dp.Precio_Venta,
           u.Nombre AS Ubicacion, a.Nombre AS Almacen
    FROM Productos p
    JOIN DetalleProductoAlmacen dp ON p.ID_Producto = dp.Id_Producto
    JOIN Ubicaciones u ON dp.Id_Ubicacion = u.ID_Ubicacion
    JOIN Almacen a ON u.ID_Almacen = a.ID_Almacen
    WHERE a.ID_Almacen = @IdAlmacen;
END;
-----------------------------------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------
	   
	   --Declarar una variable para almacenar el resultado
          DECLARE @Nombre NVARCHAR(255);

		  DECLARE @Id_Compra NVARCHAR(255);

		  DECLARE @Id_DetalleProducto NVARCHAR (50);

            -- Obtener el ID de la compra insertada
        DECLARE @IdCompra INT = SCOPE_IDENTITY();

		--Obtener el ID de la Compra creada
        DECLARE @Id_Compra INT;
        SET @IdCompra = SCOPE_IDENTITY();

-- Tabla de Compras
CREATE TABLE Compras
(
    ID_Compra int PRIMARY KEY IDENTITY(1,1),
    ID_Proveedor int,
    ID_Usuario int,
    Num_Factura varchar(15),
    Cantidad varchar(18,2),
    FechaCompra datetime,
    Subtotal decimal(18,2),
    IVA decimal(18,2),
    Total decimal(18,2),
    FOREIGN KEY (Id_Proveedor) REFERENCES Proveedores(Id_Proveedor),
    FOREIGN KEY (Id_Usuario) REFERENCES Usuarios(Id_Usuario)
);

CREATE TABLE DetalleCompra
(
    ID_Detalle_Compra int PRIMARY KEY IDENTITY(1,1),
    Id_Compra int,
	NombreProveedor int,
	NombreUsuario int ,
    Id_DetalleProducto int,
    Precio_Compra decimal(18,2),
    Cantidad int,
    Subtotal decimal(18,2),
	Total decimal (18,2)
    FOREIGN KEY (Id_DetalleProducto) REFERENCES DetalleProductoAlmacen(ID_DetalleProducto),
    FOREIGN KEY (Id_Compra) REFERENCES Compras(ID_Compra)
);

--Definir el tipo de tabla para los detalles de la compra
  CREATE TYPE CompraDetallesTypes AS TABLE(
  ID_Compra int,
  NombreProveedor int,
  NombreUsuario int,
  FechaCompra Datetime,
  precio_Compra decimal (18,2),
  Cantidad int, 
  Subtotal decimal (18,2),
  Total decimal (18, 2)
  );

  DECLARE @Id_Compra INT; -- Declarar la variable
SET @Id_Compra = 1; -- Asignar un valor 

-- se uiliza la variable en la consulta
SELECT * 
FROM Compras 
WHERE Id_Compra = @Id_Compra;

--Insertar los detalles en DetalleCompraSType
  INSERT INTO CompraDetallesTypes (ID_Compra, NombreProveedor, NombreUsuario,  FechaCompra ,Precio_Compra, Cantidad, Subtotal, Total )
  SELECT @Id_Compra, NombreProveedor, NombreUsuario
          FechaCompra,precio_Compra ,Cantidad Subtotal, Total
  FROM  @Detalles.


   --PROCEDIMIENTO PARA CREAR COMPRAS
CREATE PROCEDURE sp_CrearCompra
    @NombreProveedor NVARCHAR(100), -- Nombre del proveedor
    @NombreUsuario NVARCHAR(50),   -- Nombre del usuario
	@NombreUsuario NVARCHAR (50), --NOMBRE DEL USUARIO
	@Num_Factura NVARCHAR (50),
	@Cantidad NVARCHAR (80),
    @FechaCompra DATETIME,
    @IVA DECIMAL(18, 2),
    @Total DECIMAL(18, 2),
    @Detalles CompraDetallesTypes READONLY -- Tipo de tabla para los detalles
    AS
    BEGIN   
    SET NOCOUNT ON;

    BEGIN TRANSACTION;
    BEGIN TRY
        -- Obtener el ID del proveedor a partir de su nombre
        DECLARE @IdProveedor INT;
        SELECT @IdProveedor = ID_Proveedor
        FROM Proveedores
        WHERE Nombre = @NombreProveedor;

        IF @IdProveedor IS NULL
        BEGIN
            THROW 50001, 'Error: El proveedor especificado no existe.', 1;
        END

        -- Obtener el ID del usuario a partir de su nombre
        DECLARE @IdUsuario INT;
        SELECT @IdUsuario = ID_Usuario
        FROM Usuarios
        WHERE UsuarioName = @NombreUsuario;

        IF @IdUsuario IS NULL
        BEGIN
            THROW 50002, 'Error: El usuario especificado no existe.', 1;
        END

        -- Insertar la compra
        INSERT INTO Compras (Id_Proveedor, Id_Usuario, FechaCompra, Subtotal, IVA, Total)
        VALUES (@IdProveedor, @IdUsuario, @FechaCompra, @Subtotal, @IVA, @Total);

        -- Obtener el ID de la compra recién insertada
        DECLARE @NewIdCompra INT = SCOPE_IDENTITY();

        -- Insertar los detalles de la compra
        INSERT INTO DetalleCompra (Id_Compra, Id_DetalleProducto, Precio_Compra, Cantidad, Subtotal)
        SELECT @NewIdCompra, Id_DetalleProducto, Precio_Compra, Cantidad, Subtotal
        FROM @Detalles;

        -- Confirmar la transacción
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        -- Revertir la transacción en caso de error
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
     

--PROCECIMIENTO ALMACENADO PARA ACTUALIZAR
CREATE PROCEDURE ActualizarCompra
    @NombreProveedor NVARCHAR(100), -- Nombre del proveedor
    @NombreUsuario NVARCHAR(50),   -- Nombre del usuario
	@NombreProducto NVARCHAR (50), --NOMBRE DEL USUARIO
	@Num_Factura NVARCHAR (50),
	@Cantidad NVARCHAR (80),
    @FechaCompra DATETIME,
    @IVA DECIMAL(18, 2),
	@Subtotal DECIMAL(18, 2),
    @Total DECIMAL(18, 2),
    @Detalles CompraDetallesTypes READONLY -- Tipo de tabla para los detalles  
AS
BEGIN
    BEGIN TRANSACTION;

    BEGIN TRY    

        -- Verificar si la compra existe
        IF EXISTS (SELECT 1 FROM Compras WHERE Id_Compra = @Id_Compra)
        BEGIN
            -- Actualizar la compra
            UPDATE Compras
            SET Id_Proveedor = @NombreProveedor,
                ID_Usuario = @NombreUsuario,
				Id_Producto = @NombreProducto,
				Num_Factura = @Num_Factura,
				Cantidad = @Cantidad,
                FechaCompra = @FechaCompra,
				IVA = @IVA,
                Subtotal = @Subtotal,
                Total = @Total
            WHERE ID_Compra = @Id_Compra;

            -- Confirmar la transacción
            COMMIT TRANSACTION;

            -- Mensaje de éxito
            SELECT 'Compra actualizada exitosamente' AS Mensaje;
        END
        ELSE
        BEGIN
            -- Si la compra no existe, cancelar la transacción y devolver mensaje
            ROLLBACK TRANSACTION;
            SELECT 'Error: La compra no existe' AS Mensaje;
        END
    END TRY
    BEGIN CATCH
        -- Manejo de errores: deshacer la transacción y lanzar el error
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
	 

Select * from Compras

select * from DetallesCompra

select * from Compras where ID_Compra = @IdCompra;


    INSERT INTO DetallesCompra(Id_Compra,NombreProveedor,NombreUsuario,
		                          FechaCompra,Cantidad,Precio_Compra,Subtotal,Total)
		FROM @Detalles     
		  

       

	 


