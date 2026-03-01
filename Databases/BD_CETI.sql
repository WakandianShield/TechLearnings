-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 16-06-2025 a las 00:32:42
-- Versión del servidor: 10.4.32-MariaDB
-- Versión de PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `bd_ceti_4d`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `academia`
--

CREATE TABLE `academia` (
  `PK_Acad` tinyint(4) NOT NULL,
  `Nombre` varchar(45) DEFAULT NULL,
  `Carrera` tinyint(4) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `academia`
--

INSERT INTO `academia` (`PK_Acad`, `Nombre`, `Carrera`) VALUES
(1, 'Academia de sistemas digitales', 16),
(2, 'Academia de computacion', 16),
(3, 'Academia de sistemas electronicos', 16),
(4, 'Academia de infraestructura TI', 16),
(5, 'Academia de informatica', 16),
(6, 'Academia de humanidades', 23),
(7, 'Academia de ciencias sociales', 23),
(8, 'Academia de ingles', 23),
(9, 'Academia de lengua y comunicacion', 23),
(10, 'Academia de conciencia historica', 23),
(11, 'Academia de Ciencias Naturales, Experimentale', 22),
(12, 'Academia de fisica', 22),
(13, 'Academia de Pensamiento matematico', 22),
(14, 'Academia de dibujo', 22),
(15, 'Academia de metodologia', 22);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `alumno`
--

CREATE TABLE `alumno` (
  `Registro` bigint(20) NOT NULL,
  `Nombre` varchar(50) DEFAULT NULL,
  `Apellido` varchar(50) DEFAULT NULL,
  `Celular` bigint(20) DEFAULT NULL,
  `Domicilio` varchar(100) DEFAULT NULL,
  `Colonia` varchar(100) DEFAULT NULL,
  `Municipio` tinyint(4) DEFAULT NULL,
  `Carrera` tinyint(4) DEFAULT NULL,
  `Sexo` char(1) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `alumno`
--

INSERT INTO `alumno` (`Registro`, `Nombre`, `Apellido`, `Celular`, `Domicilio`, `Colonia`, `Municipio`, `Carrera`, `Sexo`) VALUES
(22300849, 'Zabdiel Fernando', 'Vazquez Ortiz', 3313298815, 'Elias Villalpando 1043', 'Villas del nilo', 1, 16, 'M'),
(23300732, 'Sarah Regina', 'Pacheco Venegas', 3320206006, 'Piotr I. Tchaikovski 846', 'Residencial Plaza Guadalupe', 2, 16, 'F'),
(23300745, 'Gael', 'Ruiz Alatorre', 3330106815, 'Industria Textil', 'Parques del Centinela', 2, 16, 'M'),
(23300746, 'Ivan Alexander', 'Lopez Carvajal', 3314137426, 'Av Real del bosque 108', 'Real del bosque', 2, 16, 'M'),
(23300747, 'Alonso Uriel', 'Jimenez Gutierrees', 3328056455, '193', 'Santa tere', 2, 16, 'M'),
(23300748, 'Santiago', 'Gallardo Ruvalcaba', 3333929652, 'Isla sombrero', 'Jardines de la cruz', 1, 16, 'M'),
(23300749, 'Micaela', 'Gallegos Goncalves', 3326045867, 'Calle Beethoven 287', 'Residencial Juan Manuel', 1, 16, 'F'),
(23300750, 'Cesar Augusto', 'Ramos Cruz', 3311182486, 'Presa del Infiernillo 1156', 'Lagos de Oriente', 1, 16, 'M'),
(23300755, 'Axel Ivan', 'Mora Madrigal', 3321729061, 'Calle Avestruz 97', 'Mirador de San Isidro', 2, 16, 'M'),
(23300756, 'Nadya Irina', 'Diaz Lopez', 3311431096, 'Urbano Sanroman Gomez 666', 'Residencial Poniente', 2, 16, 'F'),
(23300757, 'Kenneth Waldo', 'Ladron de Guevara Mace', 3328449369, 'Herrera y Cairo 1347', 'Santa Teresita', 1, 16, 'M'),
(23300758, 'Edgar Ruben', 'Gutierrez Corona', 3317169990, 'Calle Gilberto 867', 'Residencial Plaza Dominguez', 2, 16, 'M'),
(23300759, 'Andre Bernard', 'Heredia Perez', 3317791863, 'Volcan Pico de Quinceo 5100', 'Colli Urbano', 2, 16, 'M'),
(23300760, 'Gilberto Naim', 'Guzman Ruiz', 3331088818, '594', 'santa Tere', 1, 16, 'M'),
(23300761, 'Rodrigo Adrian', 'Salcido De Leon', 3325420189, 'Fraccionamiento Real Cantabria', 'El Tigre', 2, 16, 'M'),
(23300762, 'Gabriel Emiliano', 'Gonzalez Camacho ', 3317938735, 'Herrera y Cairo', 'Santa Fe', 2, 16, 'M'),
(23300764, 'Pablo', 'Lomeli Rodriguez', 3326048402, 'Av.Aurelio Ortega 1190', 'Colinas de atemajac', 2, 16, 'M'),
(23300765, 'Axel Leon', 'Garcia Garcia', 3321768221, 'Cuauhtemoc 476', 'Analco', 1, 16, 'M');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `asignatura`
--

CREATE TABLE `asignatura` (
  `Codigo` varchar(15) NOT NULL,
  `Nombre` varchar(60) DEFAULT NULL,
  `Academia` tinyint(4) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `asignatura`
--

INSERT INTO `asignatura` (`Codigo`, `Nombre`, `Academia`) VALUES
('233bMCLDS0301', 'Programacion Orientada a Objetos I', 2),
('233bMCLDS0302', 'Temas de electronica III', 3),
('233bMCLDS0303', 'Infraestructura de redes I', 4),
('233bMCLDS0304', 'Sistemas digitales I', 1),
('233bMCLDS0401', 'Programacion Orientada a Objetos II', 2),
('233bMCLDS0402', 'Infraestructura de redes II', 4),
('233bMCLDS0403', 'Sistemas digitales II', 1),
('233bMCLDS0404', 'Base de datos I', 5),
('30222-0003-23CF', 'Ingles III', 8),
('30222-0004-23CF', 'Ingles IV', 8),
('30224-0003-23CF', 'Lengua y comunicacion III', 9),
('30225-0001-23CF', 'Conciencia historica I', 10),
('30226-0002-23CF', 'Humanidaes II', 6),
('30310-0003-23CF', 'Ciencias sociales III', 7),
('30520-0003-23CF', 'Ecosistemas: interacciones, energia y dinamica', 11),
('30520-0004-23CF', 'Reacciones quimicas: conservacion de la materia en la formac', 11),
('30531-0003-23CF', 'Pensamiento matematico III', 13),
('30531-0004-23CF', 'Temas selectos de matematicas I', 13);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `calificacion`
--

CREATE TABLE `calificacion` (
  `PK_Calif` smallint(6) NOT NULL,
  `Calificacion` tinyint(4) DEFAULT NULL,
  `Periodo` varchar(12) DEFAULT NULL,
  `Alumno` bigint(20) DEFAULT NULL,
  `Profesor` smallint(6) DEFAULT NULL,
  `Asignatura` varchar(15) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `calificacion`
--

INSERT INTO `calificacion` (`PK_Calif`, `Calificacion`, `Periodo`, `Alumno`, `Profesor`, `Asignatura`) VALUES
(1, 100, 'AGO-DIC 2024', 23300757, 6, '233bMCLDS0303'),
(2, 99, 'AGO-DIC 2024', 23300757, 2, '233bMCLDS0301'),
(3, 100, 'AGO-DIC 2024', 23300757, 9, '30226-0002-23CF'),
(4, 100, 'AGO-DIC 2024', 23300757, 5, '233bMCLDS0302'),
(5, 100, 'AGO-DIC 2024', 23300757, 1, '233bMCLDS0304'),
(6, 100, 'AGO-DIC 2024', 23300757, 7, '30222-0003-23CF'),
(7, 100, 'AGO-DIC 2024', 23300757, 3, '30531-0003-23CF'),
(8, 99, 'AGO-DIC 2024', 23300757, 4, '30520-0003-23CF'),
(9, 100, 'AGO-DIC 2024', 23300757, 8, '30224-0003-23CF'),
(10, 97, 'FEB-JUN 2025', 23300757, 11, '233bMCLDS0402'),
(11, 100, 'FEB-JUN 2025', 23300757, 12, '233bMCLDS0401'),
(12, 94, 'FEB-JUN 2025', 23300757, 16, '30310-0003-23CF'),
(13, 100, 'FEB-JUN 2025', 23300757, 1, '233bMCLDS0403'),
(14, 95, 'FEB-JUN 2025', 23300757, 10, '233bMCLDS0404'),
(15, 100, 'FEB-JUN 2025', 23300757, 7, '30222-0004-23CF'),
(16, 100, 'FEB-JUN 2025', 23300757, 3, '30531-0004-23CF'),
(17, 100, 'FEB-JUN 2025', 23300757, 13, '30520-0004-23CF'),
(18, 96, 'FEB-JUN 2025', 23300757, 14, '30225-0001-23CF'),
(19, 100, 'AGO-DIC 2024', 23300756, 6, '233bMCLDS0303'),
(20, 94, 'AGO-DIC 2024', 23300756, 2, '233bMCLDS0301'),
(21, 99, 'AGO-DIC 2024', 23300756, 9, '30226-0002-23CF'),
(22, 96, 'AGO-DIC 2024', 23300756, 5, '233bMCLDS0302'),
(23, 97, 'AGO-DIC 2024', 23300756, 1, '233bMCLDS0304'),
(24, 77, 'AGO-DIC 2024', 23300756, 7, '30222-0003-23CF'),
(25, 93, 'AGO-DIC 2024', 23300756, 3, '30531-0003-23CF'),
(26, 93, 'AGO-DIC 2024', 23300756, 4, '30520-0003-23CF'),
(27, 82, 'AGO-DIC 2024', 23300756, 8, '30224-0003-23CF'),
(28, 94, 'FEB-JUN 2025', 23300756, 11, '233bMCLDS0402'),
(29, 88, 'FEB-JUN 2025', 23300756, 12, '233bMCLDS0401'),
(30, 100, 'FEB-JUN 2025', 23300756, 16, '30310-0003-23CF'),
(31, 99, 'FEB-JUN 2025', 23300756, 1, '233bMCLDS0403'),
(32, 92, 'FEB-JUN 2025', 23300756, 10, '233bMCLDS0404'),
(33, 91, 'FEB-JUN 2025', 23300756, 7, '30222-0004-23CF'),
(34, 99, 'FEB-JUN 2025', 23300756, 3, '30531-0004-23CF'),
(35, 100, 'FEB-JUN 2025', 23300756, 13, '30520-0004-23CF'),
(36, 92, 'FEB-JUN 2025', 23300756, 14, '30225-0001-23CF'),
(37, 94, 'AGO-DIC 2024', 23300762, 6, '233bMCLDS0303'),
(38, 89, 'AGO-DIC 2024', 23300762, 2, '233bMCLDS0301'),
(39, 96, 'AGO-DIC 2024', 23300762, 9, '30226-0002-23CF'),
(40, 99, 'AGO-DIC 2024', 23300762, 5, '233bMCLDS0302'),
(41, 90, 'AGO-DIC 2024', 23300762, 1, '233bMCLDS0304'),
(42, 88, 'AGO-DIC 2024', 23300762, 7, '30222-0003-23CF'),
(43, 90, 'AGO-DIC 2024', 23300762, 3, '30531-0003-23CF'),
(44, 83, 'AGO-DIC 2024', 23300762, 4, '30520-0003-23CF'),
(45, 86, 'AGO-DIC 2024', 23300762, 8, '30224-0003-23CF'),
(46, 60, 'FEB-JUN 2025', 23300762, 11, '233bMCLDS0402'),
(47, 80, 'FEB-JUN 2025', 23300762, 12, '233bMCLDS0401'),
(48, 90, 'FEB-JUN 2025', 23300762, 16, '30310-0003-23CF'),
(49, 94, 'FEB-JUN 2025', 23300762, 1, '233bMCLDS0403'),
(50, 95, 'FEB-JUN 2025', 23300762, 10, '233bMCLDS0404'),
(51, 78, 'FEB-JUN 2025', 23300762, 7, '30222-0004-23CF'),
(52, 95, 'FEB-JUN 2025', 23300762, 3, '30531-0004-23CF'),
(53, 88, 'FEB-JUN 2025', 23300762, 13, '30520-0004-23CF'),
(54, 90, 'FEB-JUN 2025', 23300762, 14, '30225-0001-23CF'),
(55, 99, 'AGO-DIC 2024', 23300758, 6, '233bMCLDS0303'),
(56, 99, 'AGO-DIC 2024', 23300758, 2, '233bMCLDS0301'),
(57, 94, 'AGO-DIC 2024', 23300758, 9, '30226-0002-23CF'),
(58, 97, 'AGO-DIC 2024', 23300758, 5, '233bMCLDS0302'),
(59, 92, 'AGO-DIC 2024', 23300758, 1, '233bMCLDS0304'),
(60, 97, 'AGO-DIC 2024', 23300758, 7, '30222-0003-23CF'),
(61, 100, 'AGO-DIC 2024', 23300758, 3, '30531-0003-23CF'),
(62, 90, 'AGO-DIC 2024', 23300758, 4, '30520-0003-23CF'),
(63, 97, 'AGO-DIC 2024', 23300759, 8, '30224-0003-23CF'),
(64, 92, 'FEB-JUN 2025', 23300758, 11, '233bMCLDS0402'),
(65, 94, 'FEB-JUN 2025', 23300758, 12, '233bMCLDS0401'),
(66, 94, 'FEB-JUN 2025', 23300758, 16, '30310-0003-23CF'),
(67, 91, 'FEB-JUN 2025', 23300758, 1, '233bMCLDS0403'),
(68, 95, 'FEB-JUN 2025', 23300758, 10, '233bMCLDS0404'),
(69, 85, 'FEB-JUN 2025', 23300758, 7, '30222-0004-23CF'),
(70, 90, 'FEB-JUN 2025', 23300758, 3, '30531-0004-23CF'),
(71, 96, 'FEB-JUN 2025', 23300758, 13, '30520-0004-23CF'),
(72, 85, 'FEB-JUN 2025', 23300758, 14, '30225-0001-23CF'),
(73, 99, 'AGO-DIC 2024', 23300749, 6, '233bMCLDS0303'),
(74, 95, 'AGO-DIC 2024', 23300749, 2, '233bMCLDS0301'),
(75, 96, 'AGO-DIC 2024', 23300749, 9, '30226-0002-23CF'),
(76, 99, 'AGO-DIC 2024', 23300749, 5, '233bMCLDS0302'),
(77, 100, 'AGO-DIC 2024', 23300749, 1, '233bMCLDS0304'),
(78, 88, 'AGO-DIC 2024', 23300749, 7, '30222-0003-23CF'),
(79, 100, 'AGO-DIC 2024', 23300749, 3, '30531-0003-23CF'),
(80, 91, 'AGO-DIC 2024', 23300749, 4, '30520-0003-23CF'),
(81, 97, 'AGO-DIC 2024', 23300749, 8, '30224-0003-23CF'),
(82, 95, 'FEB-JUN 2025', 23300749, 11, '233bMCLDS0402'),
(83, 96, 'FEB-JUN 2025', 23300749, 12, '233bMCLDS0401'),
(84, 100, 'FEB-JUN 2025', 23300749, 16, '30310-0003-23CF'),
(85, 100, 'FEB-JUN 2025', 23300749, 1, '233bMCLDS0403'),
(86, 95, 'FEB-JUN 2025', 23300749, 10, '233bMCLDS0404'),
(87, 76, 'FEB-JUN 2025', 23300749, 7, '30222-0004-23CF'),
(88, 100, 'FEB-JUN 2025', 23300749, 3, '30531-0004-23CF'),
(89, 98, 'FEB-JUN 2025', 23300749, 13, '30520-0004-23CF'),
(90, 100, 'FEB-JUN 2025', 23300756, 14, '30225-0001-23CF'),
(91, 99, 'AGO-DIC 2024', 23300747, 6, '233bMCLDS0303'),
(92, 97, 'AGO-DIC 2024', 23300747, 2, '233bMCLDS0301'),
(93, 98, 'AGO-DIC 2024', 23300747, 9, '30226-0002-23CF'),
(94, 97, 'AGO-DIC 2024', 23300747, 5, '233bMCLDS0302'),
(95, 99, 'AGO-DIC 2024', 23300747, 1, '233bMCLDS0304'),
(96, 99, 'AGO-DIC 2024', 23300747, 7, '30222-0003-23CF'),
(97, 100, 'AGO-DIC 2024', 23300747, 3, '30531-0003-23CF'),
(98, 94, 'AGO-DIC 2024', 23300747, 4, '30520-0003-23CF'),
(99, 97, 'AGO-DIC 2024', 23300747, 8, '30224-0003-23CF'),
(100, 94, 'FEB-JUN 2025', 23300747, 11, '233bMCLDS0402'),
(101, 82, 'FEB-JUN 2025', 23300747, 12, '233bMCLDS0401'),
(102, 94, 'FEB-JUN 2025', 23300747, 16, '30310-0003-23CF'),
(103, 100, 'FEB-JUN 2025', 23300747, 1, '233bMCLDS0403'),
(104, 95, 'FEB-JUN 2025', 23300747, 10, '233bMCLDS0404'),
(105, 99, 'FEB-JUN 2025', 23300747, 7, '30222-0004-23CF'),
(106, 99, 'FEB-JUN 2025', 23300747, 3, '30531-0004-23CF'),
(107, 100, 'FEB-JUN 2025', 23300747, 13, '30520-0004-23CF'),
(108, 95, 'FEB-JUN 2025', 23300747, 14, '30225-0001-23CF'),
(109, 99, 'AGO-DIC 2024', 23300760, 6, '233bMCLDS0303'),
(110, 74, 'AGO-DIC 2024', 23300760, 2, '233bMCLDS0301'),
(111, 87, 'AGO-DIC 2024', 23300760, 9, '30226-0002-23CF'),
(112, 89, 'AGO-DIC 2024', 23300760, 5, '233bMCLDS0302'),
(113, 70, 'AGO-DIC 2024', 23300760, 1, '233bMCLDS0304'),
(114, 88, 'AGO-DIC 2024', 23300760, 7, '30222-0003-23CF'),
(115, 75, 'AGO-DIC 2024', 23300760, 3, '30531-0003-23CF'),
(116, 86, 'AGO-DIC 2024', 23300760, 4, '30520-0003-23CF'),
(117, 91, 'AGO-DIC 2024', 23300760, 8, '30224-0003-23CF'),
(118, 85, 'FEB-JUN 2025', 23300760, 11, '233bMCLDS0402'),
(119, 82, 'FEB-JUN 2025', 23300760, 12, '233bMCLDS0401'),
(120, 85, 'FEB-JUN 2025', 23300760, 16, '30310-0003-23CF'),
(121, 36, 'FEB-JUN 2025', 23300760, 1, '233bMCLDS0403'),
(122, 90, 'FEB-JUN 2025', 23300760, 10, '233bMCLDS0404'),
(123, 80, 'FEB-JUN 2025', 23300760, 7, '30222-0004-23CF'),
(124, 82, 'FEB-JUN 2025', 23300760, 3, '30531-0004-23CF'),
(125, 88, 'FEB-JUN 2025', 23300760, 13, '30520-0004-23CF'),
(126, 85, 'FEB-JUN 2025', 23300760, 14, '30225-0001-23CF'),
(127, 99, 'AGO-DIC 2024', 23300748, 6, '233bMCLDS0303'),
(128, 95, 'AGO-DIC 2024', 23300748, 2, '233bMCLDS0301'),
(129, 95, 'AGO-DIC 2024', 23300748, 9, '30226-0002-23CF'),
(130, 99, 'AGO-DIC 2024', 23300748, 5, '233bMCLDS0302'),
(131, 93, 'AGO-DIC 2024', 23300748, 1, '233bMCLDS0304'),
(132, 96, 'AGO-DIC 2024', 23300748, 7, '30222-0003-23CF'),
(133, 94, 'AGO-DIC 2024', 23300748, 3, '30531-0003-23CF'),
(134, 89, 'AGO-DIC 2024', 23300748, 4, '30520-0003-23CF'),
(135, 98, 'AGO-DIC 2024', 23300748, 8, '30224-0003-23CF'),
(136, 94, 'FEB-JUN 2025', 23300748, 11, '233bMCLDS0402'),
(137, 92, 'FEB-JUN 2025', 23300748, 12, '233bMCLDS0401'),
(138, 94, 'FEB-JUN 2025', 23300748, 16, '30310-0003-23CF'),
(139, 98, 'FEB-JUN 2025', 23300748, 1, '233bMCLDS0403'),
(140, 90, 'FEB-JUN 2025', 23300748, 10, '233bMCLDS0404'),
(141, 91, 'FEB-JUN 2025', 23300748, 7, '30222-0004-23CF'),
(142, 92, 'FEB-JUN 2025', 23300748, 3, '30531-0004-23CF'),
(143, 98, 'FEB-JUN 2025', 23300748, 13, '30520-0004-23CF'),
(144, 83, 'FEB-JUN 2025', 23300748, 14, '30225-0001-23CF'),
(145, 95, 'AGO-DIC 2024', 23300765, 2, '233bMCLDS0301'),
(146, 100, 'AGO-DIC 2024', 23300765, 5, '233bMCLDS0302'),
(147, 96, 'AGO-DIC 2024', 23300765, 6, '233bMCLDS0303'),
(148, 89, 'AGO-DIC 2024', 23300765, 1, '233bMCLDS0304'),
(149, 98, 'AGO-DIC 2024', 23300765, 7, '30222-0003-23CF'),
(150, 96, 'AGO-DIC 2024', 23300765, 8, '30224-0003-23CF'),
(151, 86, 'AGO-DIC 2024', 23300765, 9, '30226-0002-23CF'),
(152, 88, 'AGO-DIC 2024', 23300765, 4, '30520-0003-23CF'),
(153, 87, 'AGO-DIC 2024', 23300765, 3, '30531-0003-23CF'),
(154, 96, 'FEB-JUN 2025', 23300759, 12, '233bMCLDS0401'),
(155, 89, 'FEB-JUN 2025', 23300759, 11, '233bMCLDS0402'),
(156, 94, 'FEB-JUN 2025', 23300759, 1, '233bMCLDS0403'),
(157, 92, 'FEB-JUN 2025', 23300759, 2, '233bMCLDS0404'),
(158, 65, 'FEB-JUN 2025', 23300759, 7, '30222-0004-23CF'),
(159, 63, 'FEB-JUN 2025', 23300759, 3, '30531-0004-23CF'),
(160, 100, 'FEB-JUN 2025', 23300759, 16, '30310-0003-23CF'),
(161, 67, 'FEB-JUN 2025', 23300759, 13, '30520-0004-23CF'),
(162, 85, 'FEB-JUN 2025', 23300759, 14, '30225-0001-23CF'),
(163, 83, 'AGO-DIC 2024', 23300755, 6, '233bMCLDS0303'),
(164, 76, 'AGO-DIC 2024', 23300755, 2, '233bMCLDS0301'),
(165, 87, 'AGO-DIC 2024', 23300755, 9, '30226-0002-23CF'),
(166, 82, 'AGO-DIC 2024', 23300755, 5, '233bMCLDS0302'),
(167, 70, 'AGO-DIC 2024', 23300755, 1, '233bMCLDS0304'),
(168, 97, 'AGO-DIC 2024', 23300755, 7, '30222-0003-23CF'),
(169, 80, 'AGO-DIC 2024', 23300755, 3, '30531-0003-23CF'),
(170, 85, 'AGO-DIC 2024', 23300755, 4, '30520-0003-23CF'),
(171, 71, 'AGO-DIC 2024', 23300755, 8, '30224-0003-23CF'),
(172, 63, 'FEB-JUN 2025', 23300755, 11, '233bMCLDS0402'),
(173, 84, 'FEB-JUN 2025', 23300755, 12, '233bMCLDS0401'),
(174, 74, 'FEB-JUN 2025', 23300755, 16, '30310-0003-23CF'),
(175, 30, 'FEB-JUN 2025', 23300755, 1, '233bMCLDS0403'),
(176, 90, 'FEB-JUN 2025', 23300755, 10, '233bMCLDS0404'),
(177, 78, 'FEB-JUN 2025', 23300755, 7, '30222-0004-23CF'),
(178, 72, 'FEB-JUN 2025', 23300755, 3, '30531-0004-23CF'),
(179, 97, 'FEB-JUN 2025', 23300755, 13, '30520-0004-23CF'),
(180, 90, 'FEB-JUN 2025', 23300755, 14, '30225-0001-23CF'),
(181, 98, 'AGO-DIC 2024', 23300746, 2, '233bMCLDS0301'),
(182, 100, 'AGO-DIC 2024', 23300746, 5, '233bMCLDS0302'),
(183, 100, 'AGO-DIC 2024', 23300746, 6, '233bMCLDS0303'),
(184, 99, 'AGO-DIC 2024', 23300746, 1, '233bMCLDS0304'),
(185, 99, 'AGO-DIC 2024', 23300746, 7, '30222-0003-23CF'),
(186, 99, 'AGO-DIC 2024', 23300746, 8, '30224-0003-23CF'),
(187, 98, 'AGO-DIC 2024', 23300746, 9, '30226-0002-23CF'),
(188, 92, 'AGO-DIC 2024', 23300746, 4, '30520-0003-23CF'),
(189, 94, 'AGO-DIC 2024', 23300746, 3, '30531-0003-23CF'),
(190, 96, 'FEB-JUN 2025', 23300746, 12, '233bMCLDS0401'),
(191, 95, 'FEB-JUN 2025', 23300746, 11, '233bMCLDS0402'),
(192, 98, 'FEB-JUN 2025', 23300746, 1, '233bMCLDS0403'),
(193, 95, 'FEB-JUN 2025', 23300746, 2, '233bMCLDS0404'),
(194, 96, 'FEB-JUN 2025', 23300746, 7, '30222-0004-23CF'),
(195, 98, 'FEB-JUN 2025', 23300746, 3, '30531-0004-23CF'),
(196, 100, 'FEB-JUN 2025', 23300746, 16, '30310-0003-23CF'),
(197, 98, 'FEB-JUN 2025', 23300746, 13, '30520-0004-23CF'),
(198, 91, 'FEB-JUN 2025', 23300746, 14, '30225-0001-23CF'),
(199, 98, 'AGO-DIC 2024', 23300750, 2, '233bMCLDS0301'),
(200, 100, 'AGO-DIC 2024', 23300750, 5, '233bMCLDS0302'),
(201, 98, 'AGO-DIC 2024', 23300750, 6, '233bMCLDS0303'),
(202, 99, 'AGO-DIC 2024', 23300750, 1, '233bMCLDS0304'),
(203, 98, 'AGO-DIC 2024', 23300750, 7, '30222-0003-23CF'),
(204, 98, 'AGO-DIC 2024', 23300750, 8, '30224-0003-23CF'),
(205, 98, 'AGO-DIC 2024', 23300750, 9, '30226-0002-23CF'),
(206, 94, 'AGO-DIC 2024', 23300750, 4, '30520-0003-23CF'),
(207, 100, 'AGO-DIC 2024', 23300750, 3, '30531-0003-23CF'),
(208, 96, 'FEB-JUN 2025', 23300750, 12, '233bMCLDS0401'),
(209, 93, 'FEB-JUN 2025', 23300750, 11, '233bMCLDS0402'),
(210, 97, 'FEB-JUN 2025', 23300750, 1, '233bMCLDS0403'),
(211, 95, 'FEB-JUN 2025', 23300750, 2, '233bMCLDS0404'),
(212, 90, 'FEB-JUN 2025', 23300750, 7, '30222-0004-23CF'),
(213, 94, 'FEB-JUN 2025', 23300750, 14, '30225-0001-23CF'),
(214, 100, 'FEB-JUN 2025', 23300750, 16, '30310-0003-23CF'),
(215, 100, 'FEB-JUN 2025', 23300750, 13, '30520-0004-23CF'),
(216, 100, 'FEB-JUN 2025', 23300750, 3, '30531-0004-23CF'),
(217, 95, 'AGO-DIC-2024', 23300765, 2, '233bMCLDs0301'),
(218, 100, 'AGO-DIC-2024', 23300765, 5, '233bMCLDs0302'),
(219, 96, 'AGO-DIC-2024', 23300765, 6, '233bMCLDs0303'),
(220, 89, 'AGO-DIC-2024', 23300765, 1, '233bMCLDs0304'),
(221, 98, 'AGO-DIC-2024', 23300765, 7, '30222-0003-23CF'),
(222, 96, 'AGO-DIC-2024', 23300765, 8, '30224-0003-23CF'),
(223, 86, 'AGO-DIC-2024', 23300765, 9, '30226-0002-23CF'),
(224, 88, 'AGO-DIC-2024', 23300765, 4, '30520-0003-23CF'),
(225, 87, 'AGO-DIC-2024', 23300765, 3, '30531-0003-23CF'),
(226, 40, 'FEB-JUN-2025', 23300765, 12, '233bMCLDs0401'),
(227, 86, 'FEB-JUN-2025', 23300765, 11, '233bMCLDs0402'),
(228, 97, 'FEB-JUN-2025', 23300765, 1, '233bMCLDs0403'),
(229, 90, 'FEB-JUN-2025', 23300765, 2, '233bMCLDs0404'),
(230, 89, 'FEB-JUN-2025', 23300765, 7, '30222-0004-23CF'),
(231, 100, 'FEB-JUN-2025', 23300765, 3, '30531-0004-23CF'),
(232, 94, 'FEB-JUN-2025', 23300765, 16, '30310-0003-23CF'),
(233, 100, 'FEB-JUN-2025', 23300765, 13, '30520-0004-23CF'),
(234, 91, 'FEB-JUN-2025', 23300765, 14, '30225-0001-23CF');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `carrera`
--

CREATE TABLE `carrera` (
  `Clave` tinyint(4) NOT NULL,
  `Nombre` varchar(60) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `carrera`
--

INSERT INTO `carrera` (`Clave`, `Nombre`) VALUES
(1, 'Ingenieria Bioquimica'),
(2, 'Ingenieria Industrial'),
(3, 'Ingenieria Mecatronica'),
(4, 'Ingenieria Diseno Electronico y Sistemas Inteligentes'),
(5, 'Ingenieria en Desarrollo de Software'),
(6, 'Ingenieria Civil Sustentable'),
(7, 'Ingenieria en Tecnologia de Software'),
(8, 'Tecnologo en Automatizacion y Robotica'),
(9, 'Tecnologo en Calidad y Productividad'),
(10, 'Tecnologo como Quimico en Procesos y Biotecnologia'),
(11, 'Tecnologo en Calidad Total y Productividad'),
(12, 'Tecnologo en Construccion'),
(13, 'Tecnologo en Control Automatico e Instrumentacion'),
(14, 'Tecnologo en Diseno y Mecanica Industrial'),
(15, 'Tecnologo en Desarrollo Electronico'),
(16, 'Tecnologo en Desarrollo de Software'),
(17, 'Tecnologo en Electromecanica'),
(18, 'Tecnologo en Electronica y Comunicaciones'),
(19, 'Tecnologo en Sistemas Electronicos y Telecomunicaciones'),
(20, 'Tecnologo en Mecanica Automotriz'),
(21, 'Tecnologo Mecanico en Maquinas Herramientas'),
(22, 'Ciencias basicas'),
(23, 'Ciencias administrativas');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `municipio`
--

CREATE TABLE `municipio` (
  `PK_Mun` tinyint(4) NOT NULL,
  `Nombre` varchar(60) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `municipio`
--

INSERT INTO `municipio` (`PK_Mun`, `Nombre`) VALUES
(1, 'Guadalajara'),
(2, 'Zapopan'),
(3, 'San Pedro Tlaquepaque'),
(4, 'Tonala'),
(5, 'Tlajomulco'),
(6, 'El Salto'),
(7, 'Ixtlahuacan de los Membrillos'),
(8, 'Juanacatlan'),
(9, 'Acatlan de Juarez'),
(10, 'Zapotlanejo');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `profesor`
--

CREATE TABLE `profesor` (
  `Nomina` smallint(6) NOT NULL,
  `Nombre` varchar(50) DEFAULT NULL,
  `Apellido` varchar(50) DEFAULT NULL,
  `Academia` tinyint(4) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `profesor`
--

INSERT INTO `profesor` (`Nomina`, `Nombre`, `Apellido`, `Academia`) VALUES
(1, 'Favio', 'Murillo Garcia', 1),
(2, 'Paola Fernanda', 'Ponce Villalvazo', 5),
(3, 'Martha Berenice', 'Barrios Guzman', 13),
(4, 'Edith', 'Novelo Vazquez', 11),
(5, 'Artemio', 'Beltran Bermudez', 3),
(6, 'Cruz Jannet', 'Rodriguez Chavez', 4),
(7, 'Ana Elizabeth', 'Diaz Velez Berghouse', 8),
(8, 'Maria Gabriela', 'Gonzalez Silva', 9),
(9, 'Maria del Carmen', 'Jazo Jimenez', 6),
(10, 'Luis Rene', 'Duran Hernandez', 2),
(11, 'Irma de Jesus', 'Miguel Garzon', 4),
(12, 'Sergio', 'Becerra Delgado', 5),
(13, 'Maria Guadalupe', 'Delgado Gonzalez', 11),
(14, 'Claudia Bethzabel', 'Pardo Rosales', 10),
(15, 'Erika Berenice', 'Cornejo', 13),
(16, 'Susana Guadalupe', 'Marquez Cobian', 9);

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `academia`
--
ALTER TABLE `academia`
  ADD PRIMARY KEY (`PK_Acad`),
  ADD KEY `Carrera` (`Carrera`);

--
-- Indices de la tabla `alumno`
--
ALTER TABLE `alumno`
  ADD PRIMARY KEY (`Registro`),
  ADD KEY `Municipio` (`Municipio`),
  ADD KEY `Carrera` (`Carrera`);

--
-- Indices de la tabla `asignatura`
--
ALTER TABLE `asignatura`
  ADD PRIMARY KEY (`Codigo`),
  ADD KEY `Academia` (`Academia`);

--
-- Indices de la tabla `calificacion`
--
ALTER TABLE `calificacion`
  ADD PRIMARY KEY (`PK_Calif`),
  ADD KEY `Alumno` (`Alumno`),
  ADD KEY `Profesor` (`Profesor`),
  ADD KEY `Asignatura` (`Asignatura`);

--
-- Indices de la tabla `carrera`
--
ALTER TABLE `carrera`
  ADD PRIMARY KEY (`Clave`);

--
-- Indices de la tabla `municipio`
--
ALTER TABLE `municipio`
  ADD PRIMARY KEY (`PK_Mun`);

--
-- Indices de la tabla `profesor`
--
ALTER TABLE `profesor`
  ADD PRIMARY KEY (`Nomina`),
  ADD KEY `Academia` (`Academia`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `academia`
--
ALTER TABLE `academia`
  MODIFY `PK_Acad` tinyint(4) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- AUTO_INCREMENT de la tabla `calificacion`
--
ALTER TABLE `calificacion`
  MODIFY `PK_Calif` smallint(6) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=235;

--
-- AUTO_INCREMENT de la tabla `carrera`
--
ALTER TABLE `carrera`
  MODIFY `Clave` tinyint(4) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=24;

--
-- AUTO_INCREMENT de la tabla `municipio`
--
ALTER TABLE `municipio`
  MODIFY `PK_Mun` tinyint(4) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `academia`
--
ALTER TABLE `academia`
  ADD CONSTRAINT `academia_ibfk_1` FOREIGN KEY (`Carrera`) REFERENCES `carrera` (`Clave`);

--
-- Filtros para la tabla `alumno`
--
ALTER TABLE `alumno`
  ADD CONSTRAINT `alumno_ibfk_1` FOREIGN KEY (`Municipio`) REFERENCES `municipio` (`PK_Mun`),
  ADD CONSTRAINT `alumno_ibfk_2` FOREIGN KEY (`Carrera`) REFERENCES `carrera` (`Clave`);

--
-- Filtros para la tabla `asignatura`
--
ALTER TABLE `asignatura`
  ADD CONSTRAINT `asignatura_ibfk_1` FOREIGN KEY (`Academia`) REFERENCES `academia` (`PK_Acad`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
