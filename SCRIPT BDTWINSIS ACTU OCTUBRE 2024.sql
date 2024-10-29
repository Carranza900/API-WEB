USE master
CREATE DATABASE SISTWINS
use SISTWINS

--CREACION  DE LOGIN Y SU USUARIO
create login LoginPrueba with password= 'User2024'
create user Usuario1 for login LoginPrueba

--CREACION DE LAS TABLAS

CREATE TABLE Usuarios
(
ID_Usuario int primary key identity(1,1),
UsuarioName varchar(15),
UsuarioClave varchar(20),
Id_Rol int,
Estado bit
CONSTRAINT FK_Roles FOREIGN KEY (Id_Rol) REFERENCES Roles(ID_Rol)
)

CREATE TABLE Roles
(
ID_Rol int identity(1,1) primary key,
NombreRol varchar(40),
Descripcion varchar(30),
Estado bit
)

CREATE TABLE Clientes
(
   ID_Cliente int primary key identity (1,1),
   Nombre varchar(40),
   Apellido varchar(40),
   Telefono int
)

CREATE TABLE Proveedores -- TABLA PROVEEDORES
(
ID_Proveedor int primary key identity(1,1),
NumRuc varchar(11),
Razon_Social varchar(20),
Nombre varchar(20),
Apellido varchar(20),
Telefono int,
Direccion varchar(30),
Correo varchar(40)
)

Create table Categorias
(
ID_Categoria int primary key identity(1,1),
Nombre varchar(50),
Descripcion varchar(60),
Estado bit
)

create table Productos
(
ID_Producto int primary key identity(1,1),
CódigoProducto int,
Id_Categoria int,
Nombre varchar(40),
Descripcion varchar(50),
Estado bit,
CONSTRAINT FK_Categoria FOREIGN KEY (Id_Categoria) REFERENCES Categorias(ID_Categoria)
)

Create table DetalleProducto
(
ID_DetalleProducto int primary key identity(1,1),
Id_Producto int,
Existencia int,
Ubicacion_Almacen varchar(30),
Precio_Compra decimal(18,2),
Precio_Venta decimal(18,2),
CONSTRAINT FK_Productos FOREIGN KEY (Id_Producto) REFERENCES Productos(ID_Producto)
)

--TABLAS TRANSACCIONALES 
CREATE TABLE Ventas 
(
 ID_Venta int primary key identity(1,1),
 Id_Cliente int,
 Id_Usuario int,
 Num_Factura varchar(15),
 FechaVenta datetime,
 subtotal decimal(18,2),
 IVA decimal(18,2),
 Total decimal (18,2)
 CONSTRAINT FK_Clientes FOREIGN KEY (Id_Cliente) REFERENCES Clientes(ID_Cliente),
 CONSTRAINT FK_Usuarios FOREIGN KEY (Id_Usuario) REFERENCES Usuarios(ID_Usuario)
)

CREATE TABLE DetalleVenta
(
ID_Detalle_Venta int identity(10,1) primary key,
Id_Venta int,
Id_Producto int,
Precio decimal(18,2),
Subtotal decimal(18,2)
foreign key(Id_Producto) references Productos (ID_Producto),
foreign key(Id_Venta) references Ventas (ID_Venta)
)

CREATE TABLE Compras
(
ID_Compra int identity(1,1) primary key,
Id_Proveedor int,
Id_Usuario int,
Num_Factura int,
FechaCompra datetime,
Subtotal decimal(18,2),
IVA decimal (18,2),
Total decimal (18,2)
foreign key (Id_Proveedor ) references Proveedores(ID_Proveedor),
foreign key(Id_Usuario) references Usuarios (ID_Usuario)
 )

CREATE TABLE DetalleCompra
(
ID_Detalle_Compra int identity(10,1) primary key,
Id_Compra int,
Id_Producto int,
Cantidad int,
subtotal decimal(18,2)
foreign key(Id_Producto) references Productos (ID_Producto),
foreign key(Id_Compra) references Compras (ID_Compra)
)

CREATE TABLE Devoluciones(
	ID_Devolucion int primary key identity(1,1),
	Id_Venta int,
	Id_Compra int,
	Id_Producto int ,
	Cantidad int ,
	Motivo varchar(100),
	Fecha_Devolucion datetime,
	Subtotal decimal(18,2),
	IVA decimal(18,2),
	Total decimal(18,2),
	Estado bit,
constraint FK_Ventas foreign key(Id_Venta) references Ventas(ID_Venta),
constraint FK_Compras foreign key (Id_Compra) references Compras(ID_Compra),
constraint FK_Producto foreign key (Id_Producto) references Productos(ID_Producto)
	)