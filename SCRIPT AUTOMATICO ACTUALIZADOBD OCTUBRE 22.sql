USE [master]
GO
/****** Object:  Database [SISTWINS]    Script Date: 22/10/2024 21:53:19 ******/
CREATE DATABASE [SISTWINS]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'SISTWINS', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\SISTWINS.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'SISTWINS_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\SISTWINS_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [SISTWINS] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [SISTWINS].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [SISTWINS] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [SISTWINS] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [SISTWINS] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [SISTWINS] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [SISTWINS] SET ARITHABORT OFF 
GO
ALTER DATABASE [SISTWINS] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [SISTWINS] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [SISTWINS] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [SISTWINS] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [SISTWINS] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [SISTWINS] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [SISTWINS] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [SISTWINS] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [SISTWINS] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [SISTWINS] SET  ENABLE_BROKER 
GO
ALTER DATABASE [SISTWINS] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [SISTWINS] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [SISTWINS] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [SISTWINS] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [SISTWINS] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [SISTWINS] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [SISTWINS] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [SISTWINS] SET RECOVERY FULL 
GO
ALTER DATABASE [SISTWINS] SET  MULTI_USER 
GO
ALTER DATABASE [SISTWINS] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [SISTWINS] SET DB_CHAINING OFF 
GO
ALTER DATABASE [SISTWINS] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [SISTWINS] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [SISTWINS] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [SISTWINS] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'SISTWINS', N'ON'
GO
ALTER DATABASE [SISTWINS] SET QUERY_STORE = ON
GO
ALTER DATABASE [SISTWINS] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [SISTWINS]
GO
/****** Object:  User [Usuario1]    Script Date: 22/10/2024 21:53:19 ******/
CREATE USER [Usuario1] FOR LOGIN [LoginPrueba] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  Table [dbo].[Categorias]    Script Date: 22/10/2024 21:53:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categorias](
	[ID_Categoria] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](50) NULL,
	[Descripcion] [varchar](60) NULL,
	[Estado] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_Categoria] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Clientes]    Script Date: 22/10/2024 21:53:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Clientes](
	[ID_Cliente] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](40) NULL,
	[Apellido] [varchar](40) NULL,
	[Telefono] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_Cliente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Compras]    Script Date: 22/10/2024 21:53:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Compras](
	[ID_Compra] [int] IDENTITY(1,1) NOT NULL,
	[Id_Proveedor] [int] NULL,
	[Id_Usuario] [int] NULL,
	[Num_Factura] [int] NULL,
	[FechaCompra] [datetime] NULL,
	[Subtotal] [decimal](18, 2) NULL,
	[IVA] [decimal](18, 2) NULL,
	[Total] [decimal](18, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_Compra] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DetalleCompra]    Script Date: 22/10/2024 21:53:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DetalleCompra](
	[ID_Detalle_Compra] [int] IDENTITY(10,1) NOT NULL,
	[Id_Compra] [int] NULL,
	[Id_Producto] [int] NULL,
	[Cantidad] [int] NULL,
	[subtotal] [decimal](18, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_Detalle_Compra] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DetalleProducto]    Script Date: 22/10/2024 21:53:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DetalleProducto](
	[ID_DetalleProducto] [int] IDENTITY(1,1) NOT NULL,
	[Id_Producto] [int] NULL,
	[Existencia] [int] NULL,
	[Ubicacion_Almacen] [varchar](30) NULL,
	[Precio_Compra] [decimal](18, 2) NULL,
	[Precio_Venta] [decimal](18, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_DetalleProducto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DetalleVenta]    Script Date: 22/10/2024 21:53:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DetalleVenta](
	[ID_Detalle_Venta] [int] IDENTITY(10,1) NOT NULL,
	[Id_Venta] [int] NULL,
	[Id_Producto] [int] NULL,
	[Precio] [decimal](18, 2) NULL,
	[Subtotal] [decimal](18, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_Detalle_Venta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Devoluciones]    Script Date: 22/10/2024 21:53:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Devoluciones](
	[ID_Devolucion] [int] IDENTITY(1,1) NOT NULL,
	[Id_Venta] [int] NULL,
	[Id_Compra] [int] NULL,
	[Id_Producto] [int] NULL,
	[Cantidad] [int] NULL,
	[Motivo] [varchar](100) NULL,
	[Fecha_Devolucion] [datetime] NULL,
	[Subtotal] [decimal](18, 2) NULL,
	[IVA] [decimal](18, 2) NULL,
	[Total] [decimal](18, 2) NULL,
	[Estado] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_Devolucion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Productos]    Script Date: 22/10/2024 21:53:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Productos](
	[ID_Producto] [int] IDENTITY(1,1) NOT NULL,
	[CódigoProducto] [int] NULL,
	[Id_Categoria] [int] NULL,
	[Nombre] [varchar](40) NULL,
	[Descripcion] [varchar](50) NULL,
	[Estado] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_Producto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Proveedores]    Script Date: 22/10/2024 21:53:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Proveedores](
	[ID_Proveedor] [int] IDENTITY(1,1) NOT NULL,
	[NumRuc] [varchar](11) NULL,
	[Razon_Social] [varchar](20) NULL,
	[Nombre] [varchar](20) NULL,
	[Apellido] [varchar](20) NULL,
	[Telefono] [int] NULL,
	[Direccion] [varchar](30) NULL,
	[Correo] [varchar](40) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_Proveedor] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 22/10/2024 21:53:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[ID_Rol] [int] IDENTITY(1,1) NOT NULL,
	[NombreRol] [varchar](40) NULL,
	[Descripcion] [varchar](30) NULL,
	[Estado] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_Rol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuarios]    Script Date: 22/10/2024 21:53:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuarios](
	[ID_Usuario] [int] IDENTITY(1,1) NOT NULL,
	[UsuarioName] [varchar](15) NULL,
	[UsuarioClave] [varchar](20) NULL,
	[Id_Rol] [int] NULL,
	[Estado] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_Usuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ventas]    Script Date: 22/10/2024 21:53:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ventas](
	[ID_Venta] [int] IDENTITY(1,1) NOT NULL,
	[Id_Cliente] [int] NULL,
	[Id_Usuario] [int] NULL,
	[Num_Factura] [varchar](15) NULL,
	[FechaVenta] [datetime] NULL,
	[subtotal] [decimal](18, 2) NULL,
	[IVA] [decimal](18, 2) NULL,
	[Total] [decimal](18, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_Venta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Compras]  WITH CHECK ADD FOREIGN KEY([Id_Usuario])
REFERENCES [dbo].[Usuarios] ([ID_Usuario])
GO
ALTER TABLE [dbo].[Compras]  WITH CHECK ADD FOREIGN KEY([Id_Proveedor])
REFERENCES [dbo].[Proveedores] ([ID_Proveedor])
GO
ALTER TABLE [dbo].[DetalleCompra]  WITH CHECK ADD FOREIGN KEY([Id_Compra])
REFERENCES [dbo].[Compras] ([ID_Compra])
GO
ALTER TABLE [dbo].[DetalleCompra]  WITH CHECK ADD FOREIGN KEY([Id_Producto])
REFERENCES [dbo].[Productos] ([ID_Producto])
GO
ALTER TABLE [dbo].[DetalleProducto]  WITH CHECK ADD  CONSTRAINT [FK_Productos] FOREIGN KEY([Id_Producto])
REFERENCES [dbo].[Productos] ([ID_Producto])
GO
ALTER TABLE [dbo].[DetalleProducto] CHECK CONSTRAINT [FK_Productos]
GO
ALTER TABLE [dbo].[DetalleVenta]  WITH CHECK ADD FOREIGN KEY([Id_Venta])
REFERENCES [dbo].[Ventas] ([ID_Venta])
GO
ALTER TABLE [dbo].[DetalleVenta]  WITH CHECK ADD FOREIGN KEY([Id_Producto])
REFERENCES [dbo].[Productos] ([ID_Producto])
GO
ALTER TABLE [dbo].[Devoluciones]  WITH CHECK ADD  CONSTRAINT [FK_Compras] FOREIGN KEY([Id_Compra])
REFERENCES [dbo].[Compras] ([ID_Compra])
GO
ALTER TABLE [dbo].[Devoluciones] CHECK CONSTRAINT [FK_Compras]
GO
ALTER TABLE [dbo].[Devoluciones]  WITH CHECK ADD  CONSTRAINT [FK_Producto] FOREIGN KEY([Id_Producto])
REFERENCES [dbo].[Productos] ([ID_Producto])
GO
ALTER TABLE [dbo].[Devoluciones] CHECK CONSTRAINT [FK_Producto]
GO
ALTER TABLE [dbo].[Devoluciones]  WITH CHECK ADD  CONSTRAINT [FK_Ventas] FOREIGN KEY([Id_Venta])
REFERENCES [dbo].[Ventas] ([ID_Venta])
GO
ALTER TABLE [dbo].[Devoluciones] CHECK CONSTRAINT [FK_Ventas]
GO
ALTER TABLE [dbo].[Productos]  WITH CHECK ADD  CONSTRAINT [FK_Categoria] FOREIGN KEY([Id_Categoria])
REFERENCES [dbo].[Categorias] ([ID_Categoria])
GO
ALTER TABLE [dbo].[Productos] CHECK CONSTRAINT [FK_Categoria]
GO
ALTER TABLE [dbo].[Usuarios]  WITH CHECK ADD  CONSTRAINT [FK_Roles] FOREIGN KEY([Id_Rol])
REFERENCES [dbo].[Roles] ([ID_Rol])
GO
ALTER TABLE [dbo].[Usuarios] CHECK CONSTRAINT [FK_Roles]
GO
ALTER TABLE [dbo].[Ventas]  WITH CHECK ADD  CONSTRAINT [FK_Clientes] FOREIGN KEY([Id_Cliente])
REFERENCES [dbo].[Clientes] ([ID_Cliente])
GO
ALTER TABLE [dbo].[Ventas] CHECK CONSTRAINT [FK_Clientes]
GO
ALTER TABLE [dbo].[Ventas]  WITH CHECK ADD  CONSTRAINT [FK_Usuarios] FOREIGN KEY([Id_Usuario])
REFERENCES [dbo].[Usuarios] ([ID_Usuario])
GO
ALTER TABLE [dbo].[Ventas] CHECK CONSTRAINT [FK_Usuarios]
GO
USE [master]
GO
ALTER DATABASE [SISTWINS] SET  READ_WRITE 
GO
