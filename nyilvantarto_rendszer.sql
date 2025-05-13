-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: May 13, 2025 at 02:24 AM
-- Server version: 10.4.27-MariaDB
-- PHP Version: 8.2.0

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `nyilvantarto_rendszer`
--
CREATE DATABASE IF NOT EXISTS `nyilvantarto_rendszer` DEFAULT CHARACTER SET utf8 COLLATE utf8_hungarian_ci;
USE `nyilvantarto_rendszer`;

-- --------------------------------------------------------

--
-- Table structure for table `bevetelek_kiadasok`
--

CREATE TABLE `bevetelek_kiadasok` (
  `id` int(11) NOT NULL,
  `osszeg` int(11) NOT NULL,
  `penznem` varchar(25) NOT NULL,
  `be_ki_kod` text NOT NULL,
  `teljesitesi_datum` date NOT NULL,
  `kotel_kovet_id` int(11) DEFAULT NULL,
  `gazdalkodo_szerv_id` int(11) DEFAULT NULL,
  `magan_szemely_id` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

--
-- Dumping data for table `bevetelek_kiadasok`
--

INSERT INTO `bevetelek_kiadasok` (`id`, `osszeg`, `penznem`, `be_ki_kod`, `teljesitesi_datum`, `kotel_kovet_id`, `gazdalkodo_szerv_id`, `magan_szemely_id`) VALUES
(91, 250000, 'Forint', 'Be1', '2025-05-01', NULL, 9, NULL),
(92, 75000, 'Forint', 'Ki1', '2025-05-05', 5, 11, NULL),
(93, 500000, 'Forint', 'Be2', '2025-05-10', NULL, 10, NULL),
(94, 1000, 'Euró', 'Be1', '2025-05-15', NULL, 9, NULL),
(95, 200000, 'Forint', 'Ki2', '2025-05-20', 7, 10, NULL),
(96, 350000, 'Forint', 'Be1', '2025-05-25', NULL, 11, NULL),
(397, 250000, 'Forint', 'Be1', '2025-01-05', NULL, 10, NULL),
(398, 185000, 'Forint', 'Ki1', '2025-01-10', 8, 10, NULL),
(399, 500000, 'Forint', 'Be2', '2025-01-15', NULL, 9, NULL),
(400, 1200, 'Euró', 'Be1', '2025-01-20', NULL, NULL, 13),
(401, 320000, 'Forint', 'Ki2', '2025-01-25', 7, 12, NULL),
(402, 175000, 'Forint', 'Be1', '2025-01-30', NULL, NULL, 10),
(403, 90000, 'Forint', 'Ki1', '2025-02-03', 6, NULL, 10),
(404, 450000, 'Forint', 'Be2', '2025-02-08', NULL, 11, NULL),
(405, 800, 'Euró', 'Be1', '2025-02-12', NULL, NULL, 13),
(406, 220000, 'Forint', 'Ki2', '2025-02-17', 6, 12, NULL),
(407, 275000, 'Forint', 'Be1', '2025-02-22', NULL, NULL, 9),
(408, 130000, 'Forint', 'Ki1', '2025-02-27', 5, 10, NULL),
(409, 550000, 'Forint', 'Be2', '2025-03-04', NULL, 11, NULL),
(410, 950, 'Euró', 'Be1', '2025-03-09', NULL, 13, NULL),
(411, 380000, 'Forint', 'Ki2', '2025-03-14', 6, 12, NULL),
(412, 225000, 'Forint', 'Be1', '2025-03-19', NULL, 9, NULL),
(413, 110000, 'Forint', 'Ki1', '2025-03-24', 7, 10, NULL),
(414, 600000, 'Forint', 'Be2', '2025-03-29', NULL, 11, NULL),
(415, 1050, 'Euró', 'Be1', '2025-04-03', NULL, 13, NULL),
(416, 290000, 'Forint', 'Ki2', '2025-04-08', 8, 12, NULL),
(417, 335000, 'Forint', 'Be1', '2025-04-13', NULL, 9, NULL),
(418, 150000, 'Forint', 'Ki1', '2025-04-18', 6, 10, NULL),
(419, 520000, 'Forint', 'Be2', '2025-04-23', NULL, 11, NULL),
(420, 750, 'Euró', 'Be1', '2025-04-28', NULL, 13, NULL),
(421, 310000, 'Forint', 'Ki2', '2025-05-03', 5, 12, NULL),
(422, 285000, 'Forint', 'Be1', '2025-05-08', NULL, 9, NULL),
(423, 95000, 'Forint', 'Ki1', '2025-05-13', 5, 10, NULL),
(424, 650000, 'Forint', 'Be2', '2025-05-18', NULL, 11, NULL),
(425, 1100, 'Euró', 'Be1', '2025-05-23', NULL, 13, NULL),
(427, 305000, 'Forint', 'Be1', '2025-06-02', NULL, 9, NULL),
(428, 170000, 'Forint', 'Ki1', '2025-06-07', 7, 10, NULL),
(429, 580000, 'Forint', 'Be2', '2025-06-12', NULL, 11, NULL),
(430, 850, 'Euró', 'Be1', '2025-06-17', NULL, 13, NULL),
(431, 345000, 'Forint', 'Ki2', '2025-06-22', 6, 12, NULL),
(432, 215000, 'Forint', 'Be1', '2025-06-27', NULL, 9, NULL),
(433, 120000, 'Forint', 'Ki1', '2025-07-02', 8, 10, NULL),
(434, 700000, 'Forint', 'Be2', '2025-07-07', NULL, 11, NULL),
(435, 1150, 'Euró', 'Be1', '2025-07-12', NULL, 13, NULL),
(436, 255000, 'Forint', 'Ki2', '2025-07-17', 8, 12, NULL),
(437, 365000, 'Forint', 'Be1', '2025-07-22', NULL, 9, NULL),
(438, 140000, 'Forint', 'Ki1', '2025-07-27', 5, 10, NULL),
(439, 620000, 'Forint', 'Be2', '2025-08-01', NULL, 11, NULL),
(440, 900, 'Euró', 'Be1', '2025-08-06', NULL, 13, NULL),
(441, 375000, 'Forint', 'Ki2', '2025-08-11', 9, 12, NULL),
(442, 235000, 'Forint', 'Be1', '2025-08-16', NULL, 9, NULL),
(443, 105000, 'Forint', 'Ki1', '2025-08-21', 8, 10, NULL),
(444, 750000, 'Forint', 'Be2', '2025-08-26', NULL, 11, NULL),
(445, 1250, 'Euró', 'Be1', '2025-09-01', NULL, 13, NULL),
(446, 285000, 'Forint', 'Ki2', '2025-09-06', 5, 12, NULL),
(447, 245000, 'Forint', 'Be1', '2025-09-11', NULL, 9, NULL),
(448, 165000, 'Forint', 'Ki1', '2025-09-16', 9, 10, NULL),
(449, 660000, 'Forint', 'Be2', '2025-09-21', NULL, 11, NULL),
(450, 950, 'Euró', 'Be1', '2025-09-26', NULL, 13, NULL),
(451, 405000, 'Forint', 'Ki2', '2025-10-01', 7, 12, NULL),
(452, 255000, 'Forint', 'Be1', '2025-10-06', NULL, 9, NULL),
(453, 115000, 'Forint', 'Ki1', '2025-10-11', 9, 10, NULL),
(454, 800000, 'Forint', 'Be2', '2025-10-16', NULL, 11, NULL),
(455, 1300, 'Euró', 'Be1', '2025-10-21', NULL, 13, NULL),
(456, 295000, 'Forint', 'Ki2', '2025-10-26', 8, 12, NULL),
(457, 425000, 'Forint', 'Be1', '2025-11-01', NULL, 9, NULL),
(458, 180000, 'Forint', 'Ki1', '2025-11-06', 8, 10, NULL),
(459, 700000, 'Forint', 'Be2', '2025-11-11', NULL, 11, NULL),
(460, 1000, 'Euró', 'Be1', '2025-11-16', NULL, 13, NULL),
(461, 435000, 'Forint', 'Ki2', '2025-11-21', 5, 12, NULL),
(462, 275000, 'Forint', 'Be1', '2025-11-26', NULL, 9, NULL),
(463, 125000, 'Forint', 'Ki1', '2025-12-01', 5, 10, NULL),
(464, 850000, 'Forint', 'Be2', '2025-12-06', NULL, 11, NULL),
(465, 1350, 'Euró', 'Be1', '2025-12-11', NULL, 13, NULL),
(466, 315000, 'Forint', 'Ki2', '2025-12-16', 7, 12, NULL),
(467, 465000, 'Forint', 'Be1', '2025-12-21', NULL, 9, NULL),
(468, 195000, 'Forint', 'Ki1', '2025-12-26', 6, 10, NULL),
(469, 740000, 'Forint', 'Be2', '2025-12-31', NULL, 11, NULL),
(470, 1050, 'Euró', 'Be1', '2026-01-05', NULL, 13, NULL),
(471, 355000, 'Forint', 'Ki2', '2026-01-10', 7, 12, NULL),
(472, 295000, 'Forint', 'Be1', '2026-01-15', NULL, 9, NULL),
(473, 135000, 'Forint', 'Ki1', '2026-01-20', 7, 10, NULL),
(474, 900000, 'Forint', 'Be2', '2026-01-25', NULL, 11, NULL),
(475, 1400, 'Euró', 'Be1', '2026-01-30', NULL, 13, NULL),
(476, 335000, 'Forint', 'Ki2', '2026-02-04', 7, 12, NULL),
(477, 485000, 'Forint', 'Be1', '2026-02-09', NULL, 9, NULL),
(478, 205000, 'Forint', 'Ki1', '2026-02-14', 5, 10, NULL),
(479, 780000, 'Forint', 'Be2', '2026-02-19', NULL, 11, NULL),
(480, 1100, 'Euró', 'Be1', '2026-02-24', NULL, 13, NULL),
(481, 375000, 'Forint', 'Ki2', '2026-03-01', 8, 12, NULL),
(482, 315000, 'Forint', 'Be1', '2026-03-06', NULL, 9, NULL),
(483, 145000, 'Forint', 'Ki1', '2026-03-11', 9, 10, NULL),
(484, 950000, 'Forint', 'Be2', '2026-03-16', NULL, 11, NULL),
(485, 1450, 'Euró', 'Be1', '2026-03-21', NULL, 13, NULL),
(486, 355000, 'Forint', 'Ki2', '2026-03-26', 9, 12, NULL),
(487, 505000, 'Forint', 'Be1', '2026-03-31', NULL, 9, NULL),
(488, 215000, 'Forint', 'Ki1', '2026-04-05', 6, 10, NULL),
(489, 820000, 'Forint', 'Be2', '2026-04-10', NULL, 11, NULL),
(490, 1150, 'Euró', 'Be1', '2026-04-15', NULL, 13, NULL),
(491, 395000, 'Forint', 'Ki2', '2026-04-20', 5, 12, NULL),
(492, 335000, 'Forint', 'Be1', '2026-04-25', NULL, 9, NULL),
(493, 155000, 'Forint', 'Ki1', '2026-04-30', 5, 10, NULL),
(494, 1000000, 'Forint', 'Be2', '2026-05-05', NULL, 11, NULL),
(495, 1500, 'Euró', 'Be1', '2026-05-10', NULL, 13, NULL),
(496, 375000, 'Forint', 'Ki2', '2026-05-15', 5, 12, NULL),
(617, 1800, 'Dollár', 'Be1', '2025-05-22', NULL, 11, NULL),
(618, 3500, 'Dollár', 'Be2', '2025-06-18', NULL, 10, NULL),
(619, 1400, 'Dollár', 'Ki1', '2025-07-05', 45, 9, NULL),
(620, 4800, 'Dollár', 'Ki2', '2025-07-22', 46, 12, NULL),
(621, 2200, 'Dollár', 'Be1', '2025-08-08', NULL, 11, NULL),
(622, 1750, 'Dollár', 'Ki1', '2025-08-25', 47, 11, NULL),
(623, 5200, 'Dollár', 'Be2', '2025-09-12', NULL, 10, NULL),
(624, 2400, 'Dollár', 'Ki2', '2025-09-28', 48, 9, NULL),
(625, 1850, 'Dollár', 'Be1', '2025-10-15', NULL, 12, NULL),
(626, 3800, 'Dollár', 'Be2', '2025-11-03', NULL, 11, NULL),
(627, 2950, 'Dollár', 'Ki1', '2025-11-20', 49, 10, NULL),
(628, 1650, 'Dollár', 'Be1', '2025-12-08', NULL, 11, NULL),
(629, 4250, 'Dollár', 'Ki2', '2025-12-24', 50, 9, NULL),
(630, 3300, 'Dollár', 'Be2', '2026-01-10', NULL, 11, NULL),
(631, 1900, 'Dollár', 'Ki1', '2026-01-28', 51, 12, NULL),
(632, 2750, 'Dollár', 'Be1', '2026-02-15', NULL, 10, NULL),
(633, 4900, 'Dollár', 'Ki2', '2026-03-04', 52, 11, NULL),
(634, 2150, 'Dollár', 'Be2', '2026-03-22', NULL, 9, NULL),
(635, 3650, 'Dollár', 'Ki1', '2026-04-09', 53, 11, NULL),
(636, 1950, 'Dollár', 'Be1', '2026-04-27', NULL, 12, NULL),
(637, 5300, 'Dollár', 'Ki2', '2026-05-15', 54, 10, NULL),
(638, 2850, 'Dollár', 'Be2', '2026-06-02', NULL, 11, NULL),
(639, 1700, 'Dollár', 'Ki1', '2026-06-20', 55, 9, NULL),
(640, 4600, 'Dollár', 'Be1', '2026-07-08', NULL, 11, NULL),
(641, 3100, 'Dollár', 'Ki2', '2026-07-26', 56, 12, NULL),
(642, 2050, 'Dollár', 'Be2', '2026-08-13', NULL, 10, NULL),
(643, 5100, 'Dollár', 'Ki1', '2026-08-31', 57, 11, NULL),
(644, 3750, 'Dollár', 'Be1', '2026-09-18', NULL, 9, NULL),
(645, 1550, 'Dollár', 'Ki2', '2026-10-06', 58, 11, NULL),
(646, 4100, 'Dollár', 'Be2', '2026-10-24', NULL, 12, NULL),
(647, 1500, 'Font', 'Be1', '2025-05-24', NULL, 11, NULL),
(648, 2700, 'Font', 'Be2', '2025-06-20', NULL, 12, NULL),
(649, 1300, 'Font', 'Ki1', '2025-07-07', 59, 11, NULL),
(650, 3500, 'Font', 'Ki2', '2025-07-24', 60, 10, NULL),
(651, 1900, 'Font', 'Be1', '2025-08-10', NULL, 9, NULL),
(652, 1400, 'Font', 'Ki1', '2025-08-27', 61, 11, NULL),
(653, 4100, 'Font', 'Be2', '2025-09-14', NULL, 12, NULL),
(654, 2000, 'Font', 'Ki2', '2025-09-30', 62, 11, NULL),
(655, 1600, 'Font', 'Be1', '2025-10-17', NULL, 10, NULL),
(656, 3200, 'Font', 'Be2', '2025-11-05', NULL, 9, NULL),
(657, 2500, 'Font', 'Ki1', '2025-11-22', 63, 11, NULL),
(658, 1450, 'Font', 'Be1', '2025-12-10', NULL, 12, NULL),
(659, 3800, 'Font', 'Ki2', '2025-12-26', 64, 11, NULL),
(660, 2900, 'Font', 'Be2', '2026-01-12', NULL, 10, NULL),
(661, 1650, 'Font', 'Ki1', '2026-01-30', 65, 9, NULL),
(662, 2300, 'Font', 'Be1', '2026-02-17', NULL, 11, NULL),
(663, 4300, 'Font', 'Ki2', '2026-03-06', 66, 12, NULL),
(664, 1850, 'Font', 'Be2', '2026-03-24', NULL, 11, NULL),
(665, 3100, 'Font', 'Ki1', '2026-04-11', 67, 10, NULL),
(666, 1750, 'Font', 'Be1', '2026-04-29', NULL, 9, NULL),
(667, 4500, 'Font', 'Ki2', '2026-05-17', 68, 11, NULL),
(668, 2400, 'Font', 'Be2', '2026-06-04', NULL, 12, NULL),
(669, 1350, 'Font', 'Ki1', '2026-06-22', 69, 11, NULL),
(670, 3900, 'Font', 'Be1', '2026-07-10', NULL, 10, NULL),
(671, 2800, 'Font', 'Ki2', '2026-07-28', 70, 9, NULL),
(672, 1700, 'Font', 'Be2', '2026-08-15', NULL, 11, NULL),
(673, 4200, 'Font', 'Ki1', '2026-09-02', 71, 12, NULL),
(674, 3300, 'Font', 'Be1', '2026-09-20', NULL, 11, NULL),
(675, 1250, 'Font', 'Ki2', '2026-10-08', 72, 10, NULL),
(676, 3600, 'Font', 'Be2', '2026-10-26', NULL, 9, NULL);

-- --------------------------------------------------------

--
-- Table structure for table `diagrammok`
--

CREATE TABLE `diagrammok` (
  `id` int(11) NOT NULL,
  `nev` varchar(255) NOT NULL,
  `leiras` text DEFAULT NULL,
  `diagramm_tipus` varchar(50) NOT NULL,
  `adathalmaz` varchar(50) NOT NULL,
  `diagram_ertekek_adat` longtext NOT NULL,
  `szuresi_beallitasok` text NOT NULL,
  `csoportositasi_beallitasok` text NOT NULL,
  `kijelolt_diagram_csoportositasok` text NOT NULL,
  `kijelolt_elemek_ids` text DEFAULT NULL,
  `adat_statisztika` varchar(50) DEFAULT NULL,
  `letrehozasi_datum` datetime NOT NULL,
  `modositasi_datum` datetime DEFAULT NULL,
  `letrehozo_id` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

--
-- Dumping data for table `diagrammok`
--

INSERT INTO `diagrammok` (`id`, `nev`, `leiras`, `diagramm_tipus`, `adathalmaz`, `diagram_ertekek_adat`, `szuresi_beallitasok`, `csoportositasi_beallitasok`, `kijelolt_diagram_csoportositasok`, `kijelolt_elemek_ids`, `adat_statisztika`, `letrehozasi_datum`, `modositasi_datum`, `letrehozo_id`) VALUES
(25, 'Chart_20250511_233553', 'Kötelezettségek és Követelése 2025 és 2026-os megjelenítése hónapokra bontva.', 'RowSeries', 'Koltsegvetes', '[\r\n  {\r\n    \"Title\": \"Bevételek és Kiadások - 2025\",\r\n    \"Name\": \"Év_2025\",\r\n    \"DataLabels\": true,\r\n    \"Values\": [\r\n      1431200.0,\r\n      1165800.0,\r\n      1865950.0,\r\n      1296800.0,\r\n      2985400.0,\r\n      1622050.0,\r\n      1592150.0,\r\n      2093150.0,\r\n      1370900.0,\r\n      1874750.0,\r\n      2028450.0,\r\n      2702500.0\r\n    ],\r\n    \"Fill\": \"#FF0000FF\"\r\n  },\r\n  {\r\n    \"Title\": \"Bevételek és Kiadások - 2026\",\r\n    \"Name\": \"Év_2026\",\r\n    \"DataLabels\": true,\r\n    \"Values\": [\r\n      1697200.0,\r\n      1811150.0,\r\n      2659650.0,\r\n      1931600.0,\r\n      1386300.0,\r\n      8300.0,\r\n      14400.0,\r\n      8850.0,\r\n      11250.0,\r\n      10500.0,\r\n      0.0,\r\n      0.0\r\n    ],\r\n    \"Fill\": \"#FFFF0000\"\r\n  }\r\n]', '{\r\n  \"SearchQuery\": null,\r\n  \"SelectedYear\": 0,\r\n  \"StartingDate\": \"\",\r\n  \"EndDate\": \"\",\r\n  \"IsValidStartDateExists\": false,\r\n  \"IsValidEndDateExists\": false,\r\n  \"CheckboxStatuses\": {\r\n    \"koltsegvetes_mindCB\": true,\r\n    \"koltsegvetes_idCB\": true,\r\n    \"koltsegvetes_osszegCB\": true,\r\n    \"koltsegvetes_penznemCB\": true,\r\n    \"koltsegvetes_bekikodCB\": true,\r\n    \"koltsegvetes_teljesitesiDatumCB\": true,\r\n    \"koltsegvetes_kotelKovetIDCB\": true,\r\n    \"koltsegvetes_partnerIDCB\": true,\r\n    \"kotelKovet_mindCB\": true,\r\n    \"kotelKovet_idCB\": true,\r\n    \"kotelKovet_osszegCB\": true,\r\n    \"kotelKovet_penznemCB\": true,\r\n    \"kotelKovet_tipusCB\": true,\r\n    \"kotelKovet_kifizetesHataridejeCB\": true,\r\n    \"kotelKovet_kifizetettCB\": true\r\n  }\r\n}', '{\r\n  \"GroupByPenznem\": false,\r\n  \"GroupByBeKiKod\": false,\r\n  \"GroupByKifizetett\": false,\r\n  \"GroupByMonth\": true,\r\n  \"GroupByYear\": true,\r\n  \"GroupByDate\": false,\r\n  \"SelectedCimkek\": [\r\n    \"Hónap\"\r\n  ],\r\n  \"SelectedAdatsorok\": [\r\n    \"Év\"\r\n  ],\r\n  \"SelectedDataStatistics\": \"Összeg\",\r\n  \"InnerRadius\": 0.0\r\n}', '[\r\n  {\r\n    \"Name\": \"Év_2025\",\r\n    \"Color\": {\r\n      \"A\": 255,\r\n      \"R\": 0,\r\n      \"G\": 0,\r\n      \"B\": 255\r\n    },\r\n    \"IsSelected\": true\r\n  },\r\n  {\r\n    \"Name\": \"Év_2026\",\r\n    \"Color\": {\r\n      \"A\": 255,\r\n      \"R\": 255,\r\n      \"G\": 0,\r\n      \"B\": 0\r\n    },\r\n    \"IsSelected\": true\r\n  },\r\n  {\r\n    \"Name\": \"Év_2027\",\r\n    \"Color\": {\r\n      \"A\": 255,\r\n      \"R\": 0,\r\n      \"G\": 128,\r\n      \"B\": 0\r\n    },\r\n    \"IsSelected\": false\r\n  }\r\n]', '91,92,93,94,95,96,397,398,399,400,401,402,403,404,405,406,407,408,409,410,411,412,413,414,415,416,417,418,419,420,421,422,423,424,425,426,427,428,429,430,431,432,433,434,435,436,437,438,439,440,441,442,443,444,445,446,447,448,449,450,451,452,453,454,455,456,457,458,459,460,461,462,463,464,465,466,467,468,469,470,471,472,473,474,475,476,477,478,479,480,481,482,483,484,485,486,487,488,489,490,491,492,493,494,495,496,617,618,619,620,621,622,623,624,625,626,627,628,629,630,631,632,633,634,635,636,637,638,639,640,641,642,643,644,645,646,647,648,649,650,651,652,653,654,655,656,657,658,659,660,661,662,663,664,665,666,667,668,669,670,671,672,673,674,675,676', 'Összeg', '2025-05-11 23:35:59', '2025-05-13 00:41:21', 1),
(26, 'Chart_20250511_233747', 'Bevételek és Kiadások megjelenítése Pénznemek segítségével BeKiKód-okra bontva.', 'BasicColumnSeries', 'Koltsegvetes', '[\r\n  {\r\n    \"Title\": \"Bevételek és Kiadások - Euró\",\r\n    \"Name\": \"Euró\",\r\n    \"DataLabels\": true,\r\n    \"Values\": [\r\n      1107.1428571428571,\r\n      0.0,\r\n      0.0,\r\n      0.0\r\n    ]\r\n  },\r\n  {\r\n    \"Title\": \"Bevételek és Kiadások - Font\",\r\n    \"Name\": \"Font\",\r\n    \"DataLabels\": true,\r\n    \"Values\": [\r\n      2212.5,\r\n      2806.25,\r\n      2214.2857142857142,\r\n      3164.2857142857142\r\n    ]\r\n  },\r\n  {\r\n    \"Title\": \"Bevételek és Kiadások - Dollár\",\r\n    \"Name\": \"Dollár\",\r\n    \"DataLabels\": true,\r\n    \"Values\": [\r\n      2568.75,\r\n      3368.75,\r\n      2635.7142857142858,\r\n      3757.1428571428573\r\n    ]\r\n  }\r\n]', '{\r\n  \"SearchQuery\": null,\r\n  \"SelectedYear\": 0,\r\n  \"StartingDate\": \"\",\r\n  \"EndDate\": \"\",\r\n  \"IsValidStartDateExists\": false,\r\n  \"IsValidEndDateExists\": false,\r\n  \"CheckboxStatuses\": {\r\n    \"koltsegvetes_mindCB\": true,\r\n    \"koltsegvetes_idCB\": true,\r\n    \"koltsegvetes_osszegCB\": true,\r\n    \"koltsegvetes_penznemCB\": true,\r\n    \"koltsegvetes_bekikodCB\": true,\r\n    \"koltsegvetes_teljesitesiDatumCB\": true,\r\n    \"koltsegvetes_kotelKovetIDCB\": true,\r\n    \"koltsegvetes_partnerIDCB\": true,\r\n    \"kotelKovet_mindCB\": true,\r\n    \"kotelKovet_idCB\": true,\r\n    \"kotelKovet_osszegCB\": true,\r\n    \"kotelKovet_penznemCB\": true,\r\n    \"kotelKovet_tipusCB\": true,\r\n    \"kotelKovet_kifizetesHataridejeCB\": true,\r\n    \"kotelKovet_kifizetettCB\": true\r\n  }\r\n}', '{\r\n  \"GroupByPenznem\": true,\r\n  \"GroupByBeKiKod\": true,\r\n  \"GroupByKifizetett\": false,\r\n  \"GroupByMonth\": false,\r\n  \"GroupByYear\": false,\r\n  \"GroupByDate\": false,\r\n  \"SelectedCimkek\": [\r\n    \"BeKiKód\"\r\n  ],\r\n  \"SelectedAdatsorok\": [\r\n    \"Pénznem\"\r\n  ],\r\n  \"SelectedDataStatistics\": \"Átlag\",\r\n  \"InnerRadius\": 0.0\r\n}', '[\r\n  {\r\n    \"Name\": \"Forint\",\r\n    \"Color\": {\r\n      \"A\": 255,\r\n      \"R\": 0,\r\n      \"G\": 0,\r\n      \"B\": 255\r\n    },\r\n    \"IsSelected\": false\r\n  },\r\n  {\r\n    \"Name\": \"Euró\",\r\n    \"Color\": {\r\n      \"A\": 255,\r\n      \"R\": 255,\r\n      \"G\": 0,\r\n      \"B\": 0\r\n    },\r\n    \"IsSelected\": true\r\n  },\r\n  {\r\n    \"Name\": \"Font\",\r\n    \"Color\": {\r\n      \"A\": 255,\r\n      \"R\": 75,\r\n      \"G\": 0,\r\n      \"B\": 130\r\n    },\r\n    \"IsSelected\": true\r\n  },\r\n  {\r\n    \"Name\": \"Dollár\",\r\n    \"Color\": {\r\n      \"A\": 255,\r\n      \"R\": 255,\r\n      \"G\": 0,\r\n      \"B\": 255\r\n    },\r\n    \"IsSelected\": true\r\n  }\r\n]', '91,92,93,94,95,96,397,398,399,400,401,402,403,404,405,406,407,408,409,410,411,412,413,414,415,416,417,418,419,420,421,422,423,424,425,426,427,428,429,430,431,432,433,434,435,436,437,438,439,440,441,442,443,444,445,446,447,448,449,450,451,452,453,454,455,456,457,458,459,460,461,462,463,464,465,466,467,468,469,470,471,472,473,474,475,476,477,478,479,480,481,482,483,484,485,486,487,488,489,490,491,492,493,494,495,496,617,618,619,620,621,622,623,624,625,626,627,628,629,630,631,632,633,634,635,636,637,638,639,640,641,642,643,644,645,646,647,648,649,650,651,652,653,654,655,656,657,658,659,660,661,662,663,664,665,666,667,668,669,670,671,672,673,674,675,676', 'Átlag', '2025-05-11 23:37:56', '2025-05-13 00:35:10', 1),
(27, 'Chart_20250511_233802', '2025 és 2026 bevételeinek és kiadásainak megjelenítése hónapokra bontva.', 'StackedColumnSeries', 'Koltsegvetes', '[\r\n  {\r\n    \"Title\": \"Bevételek és Kiadások - 2025\",\r\n    \"Name\": \"Év_2025\",\r\n    \"DataLabels\": true,\r\n    \"Values\": [\r\n      1431200.0,\r\n      1165800.0,\r\n      1865950.0,\r\n      1296800.0,\r\n      2985400.0,\r\n      1622050.0,\r\n      1592150.0,\r\n      2093150.0,\r\n      1370900.0,\r\n      1874750.0,\r\n      2028450.0,\r\n      2702500.0\r\n    ]\r\n  },\r\n  {\r\n    \"Title\": \"Bevételek és Kiadások - 2026\",\r\n    \"Name\": \"Év_2026\",\r\n    \"DataLabels\": true,\r\n    \"Values\": [\r\n      1697200.0,\r\n      1811150.0,\r\n      2659650.0,\r\n      1931600.0,\r\n      1386300.0,\r\n      8300.0,\r\n      14400.0,\r\n      8850.0,\r\n      11250.0,\r\n      10500.0,\r\n      0.0,\r\n      0.0\r\n    ]\r\n  }\r\n]', '{\r\n  \"SearchQuery\": \"\",\r\n  \"SelectedYear\": 0,\r\n  \"StartingDate\": \"\",\r\n  \"EndDate\": \"\",\r\n  \"IsValidStartDateExists\": false,\r\n  \"IsValidEndDateExists\": false,\r\n  \"CheckboxStatuses\": {\r\n    \"koltsegvetes_mindCB\": true,\r\n    \"koltsegvetes_idCB\": true,\r\n    \"koltsegvetes_osszegCB\": true,\r\n    \"koltsegvetes_penznemCB\": true,\r\n    \"koltsegvetes_bekikodCB\": true,\r\n    \"koltsegvetes_teljesitesiDatumCB\": true,\r\n    \"koltsegvetes_kotelKovetIDCB\": true,\r\n    \"koltsegvetes_partnerIDCB\": true,\r\n    \"kotelKovet_mindCB\": true,\r\n    \"kotelKovet_idCB\": true,\r\n    \"kotelKovet_osszegCB\": true,\r\n    \"kotelKovet_penznemCB\": true,\r\n    \"kotelKovet_tipusCB\": true,\r\n    \"kotelKovet_kifizetesHataridejeCB\": true,\r\n    \"kotelKovet_kifizetettCB\": true\r\n  }\r\n}', '{\r\n  \"GroupByPenznem\": false,\r\n  \"GroupByBeKiKod\": false,\r\n  \"GroupByKifizetett\": false,\r\n  \"GroupByMonth\": true,\r\n  \"GroupByYear\": true,\r\n  \"GroupByDate\": false,\r\n  \"SelectedCimkek\": [\r\n    \"Hónap\"\r\n  ],\r\n  \"SelectedAdatsorok\": [\r\n    \"Év\"\r\n  ],\r\n  \"SelectedDataStatistics\": \"Nincs Kiválasztva\",\r\n  \"InnerRadius\": 0.0\r\n}', '[\r\n  {\r\n    \"Name\": \"Év_2025\",\r\n    \"Color\": {\r\n      \"A\": 255,\r\n      \"R\": 0,\r\n      \"G\": 0,\r\n      \"B\": 255\r\n    },\r\n    \"IsSelected\": true\r\n  },\r\n  {\r\n    \"Name\": \"Év_2026\",\r\n    \"Color\": {\r\n      \"A\": 255,\r\n      \"R\": 255,\r\n      \"G\": 0,\r\n      \"B\": 0\r\n    },\r\n    \"IsSelected\": true\r\n  }\r\n]', '91,92,93,94,95,96,397,398,399,400,401,402,403,404,405,406,407,408,409,410,411,412,413,414,415,416,417,418,419,420,421,422,423,424,425,426,427,428,429,430,431,432,433,434,435,436,437,438,439,440,441,442,443,444,445,446,447,448,449,450,451,452,453,454,455,456,457,458,459,460,461,462,463,464,465,466,467,468,469,470,471,472,473,474,475,476,477,478,479,480,481,482,483,484,485,486,487,488,489,490,491,492,493,494,495,496,617,618,619,620,621,622,623,624,625,626,627,628,629,630,631,632,633,634,635,636,637,638,639,640,641,642,643,644,645,646,647,648,649,650,651,652,653,654,655,656,657,658,659,660,661,662,663,664,665,666,667,668,669,670,671,672,673,674,675,676', 'Nincs Kiválasztva', '2025-05-11 23:38:10', '2025-05-13 00:30:12', 2),
(29, 'Chart_20250511_234321', '2025 és 2026 Forint Bevételei', 'LineSeries', 'Koltsegvetes', '[\r\n  {\r\n    \"Title\": \"Bevételek és Kiadások - Forint + Be1 + 2025\",\r\n    \"Name\": \"Forint_Be1_2025\",\r\n    \"DataLabels\": true,\r\n    \"Values\": [\r\n      425000.0,\r\n      275000.0,\r\n      225000.0,\r\n      335000.0,\r\n      885000.0,\r\n      520000.0,\r\n      365000.0,\r\n      235000.0,\r\n      245000.0,\r\n      255000.0,\r\n      700000.0,\r\n      465000.0\r\n    ]\r\n  },\r\n  {\r\n    \"Title\": \"Bevételek és Kiadások - Forint + Be2 + 2025\",\r\n    \"Name\": \"Forint_Be2_2025\",\r\n    \"DataLabels\": true,\r\n    \"Values\": [\r\n      500000.0,\r\n      450000.0,\r\n      1150000.0,\r\n      520000.0,\r\n      1150000.0,\r\n      580000.0,\r\n      700000.0,\r\n      1370000.0,\r\n      660000.0,\r\n      800000.0,\r\n      700000.0,\r\n      1590000.0\r\n    ]\r\n  },\r\n  {\r\n    \"Title\": \"Bevételek és Kiadások - Forint + Be1 + 2026\",\r\n    \"Name\": \"Forint_Be1_2026\",\r\n    \"DataLabels\": true,\r\n    \"Values\": [\r\n      295000.0,\r\n      485000.0,\r\n      820000.0,\r\n      335000.0,\r\n      0.0,\r\n      0.0,\r\n      0.0,\r\n      0.0,\r\n      0.0,\r\n      0.0,\r\n      0.0,\r\n      0.0\r\n    ]\r\n  },\r\n  {\r\n    \"Title\": \"Bevételek és Kiadások - Forint + Be2 + 2026\",\r\n    \"Name\": \"Forint_Be2_2026\",\r\n    \"DataLabels\": true,\r\n    \"Values\": [\r\n      900000.0,\r\n      780000.0,\r\n      950000.0,\r\n      820000.0,\r\n      1000000.0,\r\n      0.0,\r\n      0.0,\r\n      0.0,\r\n      0.0,\r\n      0.0,\r\n      0.0,\r\n      0.0\r\n    ]\r\n  }\r\n]', '{\r\n  \"SearchQuery\": \"Forint\",\r\n  \"SelectedYear\": 0,\r\n  \"StartingDate\": \"\",\r\n  \"EndDate\": \"\",\r\n  \"IsValidStartDateExists\": false,\r\n  \"IsValidEndDateExists\": false,\r\n  \"CheckboxStatuses\": {\r\n    \"koltsegvetes_mindCB\": true,\r\n    \"koltsegvetes_idCB\": true,\r\n    \"koltsegvetes_osszegCB\": true,\r\n    \"koltsegvetes_penznemCB\": true,\r\n    \"koltsegvetes_bekikodCB\": true,\r\n    \"koltsegvetes_teljesitesiDatumCB\": true,\r\n    \"koltsegvetes_kotelKovetIDCB\": true,\r\n    \"koltsegvetes_partnerIDCB\": true,\r\n    \"kotelKovet_mindCB\": true,\r\n    \"kotelKovet_idCB\": true,\r\n    \"kotelKovet_osszegCB\": true,\r\n    \"kotelKovet_penznemCB\": true,\r\n    \"kotelKovet_tipusCB\": true,\r\n    \"kotelKovet_kifizetesHataridejeCB\": true,\r\n    \"kotelKovet_kifizetettCB\": true\r\n  }\r\n}', '{\r\n  \"GroupByPenznem\": true,\r\n  \"GroupByBeKiKod\": true,\r\n  \"GroupByKifizetett\": false,\r\n  \"GroupByMonth\": false,\r\n  \"GroupByYear\": false,\r\n  \"GroupByDate\": true,\r\n  \"SelectedCimkek\": [\r\n    \"Pénznem\",\r\n    \"Dátum\",\r\n    \"BeKiKód\"\r\n  ],\r\n  \"SelectedAdatsorok\": [],\r\n  \"SelectedDataStatistics\": \"Összeg\",\r\n  \"InnerRadius\": 0.0\r\n}', '[\r\n  {\r\n    \"Name\": \"Forint_Be1_2025\",\r\n    \"Color\": {\r\n      \"A\": 255,\r\n      \"R\": 0,\r\n      \"G\": 0,\r\n      \"B\": 255\r\n    },\r\n    \"IsSelected\": true\r\n  },\r\n  {\r\n    \"Name\": \"Forint_Ki1_2025\",\r\n    \"Color\": {\r\n      \"A\": 255,\r\n      \"R\": 255,\r\n      \"G\": 0,\r\n      \"B\": 0\r\n    },\r\n    \"IsSelected\": false\r\n  },\r\n  {\r\n    \"Name\": \"Forint_Be2_2025\",\r\n    \"Color\": {\r\n      \"A\": 255,\r\n      \"R\": 0,\r\n      \"G\": 128,\r\n      \"B\": 0\r\n    },\r\n    \"IsSelected\": true\r\n  },\r\n  {\r\n    \"Name\": \"Forint_Ki2_2025\",\r\n    \"Color\": {\r\n      \"A\": 255,\r\n      \"R\": 255,\r\n      \"G\": 0,\r\n      \"B\": 255\r\n    },\r\n    \"IsSelected\": false\r\n  },\r\n  {\r\n    \"Name\": \"Forint_Ki2_2026\",\r\n    \"Color\": {\r\n      \"A\": 255,\r\n      \"R\": 0,\r\n      \"G\": 0,\r\n      \"B\": 255\r\n    },\r\n    \"IsSelected\": false\r\n  },\r\n  {\r\n    \"Name\": \"Forint_Be1_2026\",\r\n    \"Color\": {\r\n      \"A\": 255,\r\n      \"R\": 255,\r\n      \"G\": 0,\r\n      \"B\": 0\r\n    },\r\n    \"IsSelected\": true\r\n  },\r\n  {\r\n    \"Name\": \"Forint_Ki1_2026\",\r\n    \"Color\": {\r\n      \"A\": 255,\r\n      \"R\": 0,\r\n      \"G\": 128,\r\n      \"B\": 0\r\n    },\r\n    \"IsSelected\": false\r\n  },\r\n  {\r\n    \"Name\": \"Forint_Be2_2026\",\r\n    \"Color\": {\r\n      \"A\": 255,\r\n      \"R\": 255,\r\n      \"G\": 0,\r\n      \"B\": 255\r\n    },\r\n    \"IsSelected\": true\r\n  }\r\n]', '91,92,93,95,96,397,398,399,401,402,403,404,406,407,408,409,411,412,413,414,416,417,418,419,421,422,423,424,426,427,428,429,431,432,433,434,436,437,438,439,441,442,443,444,446,447,448,449,451,452,453,454,456,457,458,459,461,462,463,464,466,467,468,469,471,472,473,474,476,477,478,479,481,482,483,484,486,487,488,489,491,492,493,494,496', 'Összeg', '2025-05-11 23:43:28', '2025-05-13 00:14:40', 1),
(31, 'Chart_20250511_235201', 'Forint-ban várt kötelezettségek és követelések évekre bontott megjelenítése a cég megléte óta.', 'DoughnutSeries', 'KotelezettsegKoveteles', '[\r\n  {\r\n    \"Title\": \"Kötelezettségek és Követelések - Forint_2025\",\r\n    \"Name\": \"Forint_2025\",\r\n    \"DataLabels\": true,\r\n    \"Values\": [\r\n      12260000.0\r\n    ],\r\n    \"Fill\": \"#FF0000FF\"\r\n  },\r\n  {\r\n    \"Title\": \"Kötelezettségek és Követelések - Forint_2026\",\r\n    \"Name\": \"Forint_2026\",\r\n    \"DataLabels\": true,\r\n    \"Values\": [\r\n      13170000.0\r\n    ],\r\n    \"Fill\": \"#FFFF0000\"\r\n  },\r\n  {\r\n    \"Title\": \"Kötelezettségek és Követelések - Forint_2027\",\r\n    \"Name\": \"Forint_2027\",\r\n    \"DataLabels\": true,\r\n    \"Values\": [\r\n      5190000.0\r\n    ],\r\n    \"Fill\": \"#FF008000\"\r\n  }\r\n]', '{\r\n  \"SearchQuery\": \"Forint\",\r\n  \"SelectedYear\": 0,\r\n  \"StartingDate\": \"\",\r\n  \"EndDate\": \"\",\r\n  \"IsValidStartDateExists\": false,\r\n  \"IsValidEndDateExists\": false,\r\n  \"CheckboxStatuses\": {\r\n    \"koltsegvetes_mindCB\": true,\r\n    \"koltsegvetes_idCB\": true,\r\n    \"koltsegvetes_osszegCB\": true,\r\n    \"koltsegvetes_penznemCB\": true,\r\n    \"koltsegvetes_bekikodCB\": true,\r\n    \"koltsegvetes_teljesitesiDatumCB\": true,\r\n    \"koltsegvetes_kotelKovetIDCB\": true,\r\n    \"koltsegvetes_partnerIDCB\": true,\r\n    \"kotelKovet_mindCB\": true,\r\n    \"kotelKovet_idCB\": true,\r\n    \"kotelKovet_osszegCB\": true,\r\n    \"kotelKovet_penznemCB\": true,\r\n    \"kotelKovet_tipusCB\": true,\r\n    \"kotelKovet_kifizetesHataridejeCB\": true,\r\n    \"kotelKovet_kifizetettCB\": true\r\n  }\r\n}', '{\r\n  \"GroupByPenznem\": true,\r\n  \"GroupByBeKiKod\": true,\r\n  \"GroupByKifizetett\": false,\r\n  \"GroupByMonth\": false,\r\n  \"GroupByYear\": true,\r\n  \"GroupByDate\": false,\r\n  \"SelectedCimkek\": [\r\n    \"BeKiKód\",\r\n    \"Pénznem\",\r\n    \"Év\"\r\n  ],\r\n  \"SelectedAdatsorok\": [],\r\n  \"SelectedDataStatistics\": \"Összeg\",\r\n  \"InnerRadius\": 0.0\r\n}', '[\r\n  {\r\n    \"Name\": \"Forint_2025\",\r\n    \"Color\": {\r\n      \"A\": 255,\r\n      \"R\": 0,\r\n      \"G\": 0,\r\n      \"B\": 255\r\n    },\r\n    \"IsSelected\": true\r\n  },\r\n  {\r\n    \"Name\": \"Forint_2026\",\r\n    \"Color\": {\r\n      \"A\": 255,\r\n      \"R\": 255,\r\n      \"G\": 0,\r\n      \"B\": 0\r\n    },\r\n    \"IsSelected\": true\r\n  },\r\n  {\r\n    \"Name\": \"Forint_2027\",\r\n    \"Color\": {\r\n      \"A\": 255,\r\n      \"R\": 0,\r\n      \"G\": 128,\r\n      \"B\": 0\r\n    },\r\n    \"IsSelected\": true\r\n  }\r\n]', '5,6,7,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,53,54,56,57,59,60,62,63,65,66,68,69,71,72,74,75,77,78,80,81,83,84,86,87,89,90,92,93,95,96,98,99,101,102,104,105,107,108', 'Összeg', '2025-05-11 23:52:11', '2025-05-13 00:26:35', 1),
(32, 'Chart_20250512_000650', '2025 kötelezettség és követelései kifizetett állapot szerinti felbontásban', 'DoughnutSeries', 'KotelezettsegKoveteles', '[\r\n  {\r\n    \"Title\": \"Kötelezettségek és Követelések - 0_2025\",\r\n    \"Name\": \"Kifizetett_0_2025\",\r\n    \"DataLabels\": true,\r\n    \"Values\": [\r\n      6350.0\r\n    ],\r\n    \"Fill\": \"#FF0000FF\"\r\n  },\r\n  {\r\n    \"Title\": \"Kötelezettségek és Követelések - 1_2025\",\r\n    \"Name\": \"Kifizetett_1_2025\",\r\n    \"DataLabels\": true,\r\n    \"Values\": [\r\n      2000.0\r\n    ],\r\n    \"Fill\": \"#FFFF0000\"\r\n  }\r\n]', '{\r\n  \"SearchQuery\": \"Euró\",\r\n  \"SelectedYear\": 0,\r\n  \"StartingDate\": \"\",\r\n  \"EndDate\": \"\",\r\n  \"IsValidStartDateExists\": false,\r\n  \"IsValidEndDateExists\": false,\r\n  \"CheckboxStatuses\": {\r\n    \"koltsegvetes_mindCB\": true,\r\n    \"koltsegvetes_idCB\": true,\r\n    \"koltsegvetes_osszegCB\": true,\r\n    \"koltsegvetes_penznemCB\": true,\r\n    \"koltsegvetes_bekikodCB\": true,\r\n    \"koltsegvetes_teljesitesiDatumCB\": true,\r\n    \"koltsegvetes_kotelKovetIDCB\": true,\r\n    \"koltsegvetes_partnerIDCB\": true,\r\n    \"kotelKovet_mindCB\": true,\r\n    \"kotelKovet_idCB\": true,\r\n    \"kotelKovet_osszegCB\": true,\r\n    \"kotelKovet_penznemCB\": true,\r\n    \"kotelKovet_tipusCB\": true,\r\n    \"kotelKovet_kifizetesHataridejeCB\": true,\r\n    \"kotelKovet_kifizetettCB\": true\r\n  }\r\n}', '{\r\n  \"GroupByPenznem\": false,\r\n  \"GroupByBeKiKod\": true,\r\n  \"GroupByKifizetett\": true,\r\n  \"GroupByMonth\": false,\r\n  \"GroupByYear\": true,\r\n  \"GroupByDate\": false,\r\n  \"SelectedCimkek\": [\r\n    \"BeKiKód\",\r\n    \"Év\",\r\n    \"Kifizetett\"\r\n  ],\r\n  \"SelectedAdatsorok\": [],\r\n  \"SelectedDataStatistics\": \"Összeg\",\r\n  \"InnerRadius\": 0.0\r\n}', '[\r\n  {\r\n    \"Name\": \"Kifizetett_0_2025\",\r\n    \"Color\": {\r\n      \"A\": 255,\r\n      \"R\": 0,\r\n      \"G\": 0,\r\n      \"B\": 255\r\n    },\r\n    \"IsSelected\": true\r\n  },\r\n  {\r\n    \"Name\": \"Kifizetett_1_2025\",\r\n    \"Color\": {\r\n      \"A\": 255,\r\n      \"R\": 255,\r\n      \"G\": 0,\r\n      \"B\": 0\r\n    },\r\n    \"IsSelected\": true\r\n  },\r\n  {\r\n    \"Name\": \"Kifizetett_0_2026\",\r\n    \"Color\": {\r\n      \"A\": 255,\r\n      \"R\": 0,\r\n      \"G\": 128,\r\n      \"B\": 0\r\n    },\r\n    \"IsSelected\": false\r\n  },\r\n  {\r\n    \"Name\": \"Kifizetett_0_2027\",\r\n    \"Color\": {\r\n      \"A\": 255,\r\n      \"R\": 255,\r\n      \"G\": 0,\r\n      \"B\": 255\r\n    },\r\n    \"IsSelected\": false\r\n  }\r\n]', '8,52,55,58,61,64,67,70,73,76,79,82,85,88,91,94,97,100,103,106,109', 'Összeg', '2025-05-12 00:06:58', '2025-05-13 00:24:47', 2),
(33, 'Chart_20250512_001229', 'Font bevételek és kiadások a cég teljes élettartalma alatt', 'DoughnutSeries', 'Koltsegvetes', '[\r\n  {\r\n    \"Title\": \"Bevételek és Kiadások - Font_Be1\",\r\n    \"Name\": \"Font_Be1\",\r\n    \"DataLabels\": true,\r\n    \"Values\": [\r\n      17700.0\r\n    ],\r\n    \"Fill\": \"#FF0000FF\"\r\n  },\r\n  {\r\n    \"Title\": \"Bevételek és Kiadások - Font_Be2\",\r\n    \"Name\": \"Font_Be2\",\r\n    \"DataLabels\": true,\r\n    \"Values\": [\r\n      22450.0\r\n    ],\r\n    \"Fill\": \"#FFFF0000\"\r\n  },\r\n  {\r\n    \"Title\": \"Bevételek és Kiadások - Font_Ki1\",\r\n    \"Name\": \"Font_Ki1\",\r\n    \"DataLabels\": true,\r\n    \"Values\": [\r\n      15500.0\r\n    ],\r\n    \"Fill\": \"#FF008000\"\r\n  },\r\n  {\r\n    \"Title\": \"Bevételek és Kiadások - Font_Ki2\",\r\n    \"Name\": \"Font_Ki2\",\r\n    \"DataLabels\": true,\r\n    \"Values\": [\r\n      22150.0\r\n    ],\r\n    \"Fill\": \"#FFFF00FF\"\r\n  }\r\n]', '{\r\n  \"SearchQuery\": \"Font\",\r\n  \"SelectedYear\": 0,\r\n  \"StartingDate\": \"\",\r\n  \"EndDate\": \"\",\r\n  \"IsValidStartDateExists\": false,\r\n  \"IsValidEndDateExists\": false,\r\n  \"CheckboxStatuses\": {\r\n    \"koltsegvetes_mindCB\": true,\r\n    \"koltsegvetes_idCB\": true,\r\n    \"koltsegvetes_osszegCB\": true,\r\n    \"koltsegvetes_penznemCB\": true,\r\n    \"koltsegvetes_bekikodCB\": true,\r\n    \"koltsegvetes_teljesitesiDatumCB\": true,\r\n    \"koltsegvetes_kotelKovetIDCB\": true,\r\n    \"koltsegvetes_partnerIDCB\": true,\r\n    \"kotelKovet_mindCB\": true,\r\n    \"kotelKovet_idCB\": true,\r\n    \"kotelKovet_osszegCB\": true,\r\n    \"kotelKovet_penznemCB\": true,\r\n    \"kotelKovet_tipusCB\": true,\r\n    \"kotelKovet_kifizetesHataridejeCB\": true,\r\n    \"kotelKovet_kifizetettCB\": true\r\n  }\r\n}', '{\r\n  \"GroupByPenznem\": true,\r\n  \"GroupByBeKiKod\": true,\r\n  \"GroupByKifizetett\": false,\r\n  \"GroupByMonth\": false,\r\n  \"GroupByYear\": false,\r\n  \"GroupByDate\": false,\r\n  \"SelectedCimkek\": [\r\n    \"BeKiKód\",\r\n    \"Pénznem\"\r\n  ],\r\n  \"SelectedAdatsorok\": [],\r\n  \"SelectedDataStatistics\": \"Összeg\",\r\n  \"InnerRadius\": 25.054945054945051\r\n}', '[\r\n  {\r\n    \"Name\": \"Font_Be1\",\r\n    \"Color\": {\r\n      \"A\": 255,\r\n      \"R\": 0,\r\n      \"G\": 0,\r\n      \"B\": 255\r\n    },\r\n    \"IsSelected\": true\r\n  },\r\n  {\r\n    \"Name\": \"Font_Be2\",\r\n    \"Color\": {\r\n      \"A\": 255,\r\n      \"R\": 255,\r\n      \"G\": 0,\r\n      \"B\": 0\r\n    },\r\n    \"IsSelected\": true\r\n  },\r\n  {\r\n    \"Name\": \"Font_Ki1\",\r\n    \"Color\": {\r\n      \"A\": 255,\r\n      \"R\": 0,\r\n      \"G\": 128,\r\n      \"B\": 0\r\n    },\r\n    \"IsSelected\": true\r\n  },\r\n  {\r\n    \"Name\": \"Font_Ki2\",\r\n    \"Color\": {\r\n      \"A\": 255,\r\n      \"R\": 255,\r\n      \"G\": 0,\r\n      \"B\": 255\r\n    },\r\n    \"IsSelected\": true\r\n  }\r\n]', '647,648,649,650,651,652,653,654,655,656,657,658,659,660,661,662,663,664,665,666,667,668,669,670,671,672,673,674,675,676', 'Összeg', '2025-05-12 00:12:54', '2025-05-13 00:21:39', 1),
(34, 'Chart_20250512_025241', '2025 dollár kiadásainak megjelenítése havi bontásban', 'DoughnutSeries', 'Koltsegvetes', '[\r\n  {\r\n    \"Title\": \"Bevételek és Kiadások - Ki1_7\",\r\n    \"Name\": \"Ki1_7\",\r\n    \"DataLabels\": true,\r\n    \"Values\": [\r\n      1400.0\r\n    ],\r\n    \"Fill\": \"#FFFFE4C4\"\r\n  },\r\n  {\r\n    \"Title\": \"Bevételek és Kiadások - Ki2_7\",\r\n    \"Name\": \"Ki2_7\",\r\n    \"DataLabels\": true,\r\n    \"Values\": [\r\n      4800.0\r\n    ],\r\n    \"Fill\": \"#FFFF00FF\"\r\n  },\r\n  {\r\n    \"Title\": \"Bevételek és Kiadások - Ki1_8\",\r\n    \"Name\": \"Ki1_8\",\r\n    \"DataLabels\": true,\r\n    \"Values\": [\r\n      1750.0\r\n    ],\r\n    \"Fill\": \"#FF008000\"\r\n  },\r\n  {\r\n    \"Title\": \"Bevételek és Kiadások - Ki2_9\",\r\n    \"Name\": \"Ki2_9\",\r\n    \"DataLabels\": true,\r\n    \"Values\": [\r\n      2400.0\r\n    ],\r\n    \"Fill\": \"#FFFF0000\"\r\n  },\r\n  {\r\n    \"Title\": \"Bevételek és Kiadások - Ki1_11\",\r\n    \"Name\": \"Ki1_11\",\r\n    \"DataLabels\": true,\r\n    \"Values\": [\r\n      2950.0\r\n    ],\r\n    \"Fill\": \"#FF00FFFF\"\r\n  },\r\n  {\r\n    \"Title\": \"Bevételek és Kiadások - Ki2_12\",\r\n    \"Name\": \"Ki2_12\",\r\n    \"DataLabels\": true,\r\n    \"Values\": [\r\n      4250.0\r\n    ],\r\n    \"Fill\": \"#FFF0FFFF\"\r\n  }\r\n]', '{\r\n  \"SearchQuery\": \"Dollár\",\r\n  \"SelectedYear\": 2025,\r\n  \"StartingDate\": \"\",\r\n  \"EndDate\": \"\",\r\n  \"IsValidStartDateExists\": false,\r\n  \"IsValidEndDateExists\": false,\r\n  \"CheckboxStatuses\": {\r\n    \"koltsegvetes_mindCB\": true,\r\n    \"koltsegvetes_idCB\": true,\r\n    \"koltsegvetes_osszegCB\": true,\r\n    \"koltsegvetes_penznemCB\": true,\r\n    \"koltsegvetes_bekikodCB\": true,\r\n    \"koltsegvetes_teljesitesiDatumCB\": true,\r\n    \"koltsegvetes_kotelKovetIDCB\": true,\r\n    \"koltsegvetes_partnerIDCB\": true,\r\n    \"kotelKovet_mindCB\": true,\r\n    \"kotelKovet_idCB\": true,\r\n    \"kotelKovet_osszegCB\": true,\r\n    \"kotelKovet_penznemCB\": true,\r\n    \"kotelKovet_tipusCB\": true,\r\n    \"kotelKovet_kifizetesHataridejeCB\": true,\r\n    \"kotelKovet_kifizetettCB\": true\r\n  }\r\n}', '{\r\n  \"GroupByPenznem\": false,\r\n  \"GroupByBeKiKod\": true,\r\n  \"GroupByKifizetett\": false,\r\n  \"GroupByMonth\": true,\r\n  \"GroupByYear\": false,\r\n  \"GroupByDate\": false,\r\n  \"SelectedCimkek\": [\r\n    \"Hónap\",\r\n    \"BeKiKód\"\r\n  ],\r\n  \"SelectedAdatsorok\": [],\r\n  \"SelectedDataStatistics\": \"Összeg\",\r\n  \"InnerRadius\": 24.615384615384613\r\n}', '[\r\n  {\r\n    \"Name\": \"Be1_5\",\r\n    \"Color\": {\r\n      \"A\": 255,\r\n      \"R\": 0,\r\n      \"G\": 0,\r\n      \"B\": 255\r\n    },\r\n    \"IsSelected\": false\r\n  },\r\n  {\r\n    \"Name\": \"Be2_6\",\r\n    \"Color\": {\r\n      \"A\": 255,\r\n      \"R\": 255,\r\n      \"G\": 0,\r\n      \"B\": 0\r\n    },\r\n    \"IsSelected\": false\r\n  },\r\n  {\r\n    \"Name\": \"Ki1_7\",\r\n    \"Color\": {\r\n      \"A\": 255,\r\n      \"R\": 255,\r\n      \"G\": 228,\r\n      \"B\": 196\r\n    },\r\n    \"IsSelected\": true\r\n  },\r\n  {\r\n    \"Name\": \"Ki2_7\",\r\n    \"Color\": {\r\n      \"A\": 255,\r\n      \"R\": 255,\r\n      \"G\": 0,\r\n      \"B\": 255\r\n    },\r\n    \"IsSelected\": true\r\n  },\r\n  {\r\n    \"Name\": \"Be1_8\",\r\n    \"Color\": {\r\n      \"A\": 255,\r\n      \"R\": 255,\r\n      \"G\": 0,\r\n      \"B\": 0\r\n    },\r\n    \"IsSelected\": false\r\n  },\r\n  {\r\n    \"Name\": \"Ki1_8\",\r\n    \"Color\": {\r\n      \"A\": 255,\r\n      \"R\": 0,\r\n      \"G\": 128,\r\n      \"B\": 0\r\n    },\r\n    \"IsSelected\": true\r\n  },\r\n  {\r\n    \"Name\": \"Be2_9\",\r\n    \"Color\": {\r\n      \"A\": 255,\r\n      \"R\": 255,\r\n      \"G\": 0,\r\n      \"B\": 255\r\n    },\r\n    \"IsSelected\": false\r\n  },\r\n  {\r\n    \"Name\": \"Ki2_9\",\r\n    \"Color\": {\r\n      \"A\": 255,\r\n      \"R\": 255,\r\n      \"G\": 0,\r\n      \"B\": 0\r\n    },\r\n    \"IsSelected\": true\r\n  },\r\n  {\r\n    \"Name\": \"Be1_10\",\r\n    \"Color\": {\r\n      \"A\": 255,\r\n      \"R\": 0,\r\n      \"G\": 128,\r\n      \"B\": 0\r\n    },\r\n    \"IsSelected\": false\r\n  },\r\n  {\r\n    \"Name\": \"Be2_11\",\r\n    \"Color\": {\r\n      \"A\": 255,\r\n      \"R\": 255,\r\n      \"G\": 0,\r\n      \"B\": 255\r\n    },\r\n    \"IsSelected\": false\r\n  },\r\n  {\r\n    \"Name\": \"Ki1_11\",\r\n    \"Color\": {\r\n      \"A\": 255,\r\n      \"R\": 0,\r\n      \"G\": 255,\r\n      \"B\": 255\r\n    },\r\n    \"IsSelected\": true\r\n  },\r\n  {\r\n    \"Name\": \"Be1_12\",\r\n    \"Color\": {\r\n      \"A\": 255,\r\n      \"R\": 0,\r\n      \"G\": 128,\r\n      \"B\": 0\r\n    },\r\n    \"IsSelected\": false\r\n  },\r\n  {\r\n    \"Name\": \"Ki2_12\",\r\n    \"Color\": {\r\n      \"A\": 255,\r\n      \"R\": 240,\r\n      \"G\": 255,\r\n      \"B\": 255\r\n    },\r\n    \"IsSelected\": true\r\n  }\r\n]', '617,618,619,620,621,622,623,624,625,626,627,628,629,630,631,632,633,634,635,636,637,638,639,640,641,642,643,644,645,646', 'Összeg', '2025-05-12 02:53:48', '2025-05-13 00:20:19', 1),
(35, 'Chart_20250512_205948', '2025 és 2026 Forint forgalma - bevétel és kiadás egyben', 'LineSeries', 'Koltsegvetes', '[\r\n  {\r\n    \"Title\": \"Bevételek és Kiadások - Forint + 2025\",\r\n    \"Name\": \"Forint_2025\",\r\n    \"DataLabels\": true,\r\n    \"Values\": [\r\n      1430000.0,\r\n      1165000.0,\r\n      1865000.0,\r\n      1295000.0,\r\n      2980000.0,\r\n      1615000.0,\r\n      1580000.0,\r\n      2085000.0,\r\n      1355000.0,\r\n      1870000.0,\r\n      2015000.0,\r\n      2690000.0\r\n    ]\r\n  },\r\n  {\r\n    \"Title\": \"Bevételek és Kiadások - Forint + 2026\",\r\n    \"Name\": \"Forint_2026\",\r\n    \"DataLabels\": true,\r\n    \"Values\": [\r\n      1685000.0,\r\n      1805000.0,\r\n      2645000.0,\r\n      1920000.0,\r\n      1375000.0,\r\n      0.0,\r\n      0.0,\r\n      0.0,\r\n      0.0,\r\n      0.0,\r\n      0.0,\r\n      0.0\r\n    ]\r\n  }\r\n]', '{\r\n  \"SearchQuery\": null,\r\n  \"SelectedYear\": 0,\r\n  \"StartingDate\": \"\",\r\n  \"EndDate\": \"\",\r\n  \"IsValidStartDateExists\": false,\r\n  \"IsValidEndDateExists\": false,\r\n  \"CheckboxStatuses\": {\r\n    \"koltsegvetes_mindCB\": true,\r\n    \"koltsegvetes_idCB\": true,\r\n    \"koltsegvetes_osszegCB\": true,\r\n    \"koltsegvetes_penznemCB\": true,\r\n    \"koltsegvetes_bekikodCB\": true,\r\n    \"koltsegvetes_teljesitesiDatumCB\": true,\r\n    \"koltsegvetes_kotelKovetIDCB\": true,\r\n    \"koltsegvetes_partnerIDCB\": true,\r\n    \"kotelKovet_mindCB\": true,\r\n    \"kotelKovet_idCB\": true,\r\n    \"kotelKovet_osszegCB\": true,\r\n    \"kotelKovet_penznemCB\": true,\r\n    \"kotelKovet_tipusCB\": true,\r\n    \"kotelKovet_kifizetesHataridejeCB\": true,\r\n    \"kotelKovet_kifizetettCB\": true\r\n  }\r\n}', '{\r\n  \"GroupByPenznem\": true,\r\n  \"GroupByBeKiKod\": false,\r\n  \"GroupByKifizetett\": false,\r\n  \"GroupByMonth\": false,\r\n  \"GroupByYear\": false,\r\n  \"GroupByDate\": true,\r\n  \"SelectedCimkek\": [\r\n    \"Pénznem\",\r\n    \"Dátum\"\r\n  ],\r\n  \"SelectedAdatsorok\": [],\r\n  \"SelectedDataStatistics\": \"Összeg\",\r\n  \"InnerRadius\": 0.0\r\n}', '[\r\n  {\r\n    \"Name\": \"Forint_2025\",\r\n    \"Color\": {\r\n      \"A\": 255,\r\n      \"R\": 0,\r\n      \"G\": 0,\r\n      \"B\": 255\r\n    },\r\n    \"IsSelected\": true\r\n  },\r\n  {\r\n    \"Name\": \"Euró_2025\",\r\n    \"Color\": {\r\n      \"A\": 255,\r\n      \"R\": 255,\r\n      \"G\": 0,\r\n      \"B\": 0\r\n    },\r\n    \"IsSelected\": false\r\n  },\r\n  {\r\n    \"Name\": \"Euró_2026\",\r\n    \"Color\": {\r\n      \"A\": 255,\r\n      \"R\": 0,\r\n      \"G\": 128,\r\n      \"B\": 0\r\n    },\r\n    \"IsSelected\": false\r\n  },\r\n  {\r\n    \"Name\": \"Forint_2026\",\r\n    \"Color\": {\r\n      \"A\": 255,\r\n      \"R\": 255,\r\n      \"G\": 0,\r\n      \"B\": 255\r\n    },\r\n    \"IsSelected\": true\r\n  },\r\n  {\r\n    \"Name\": \"Dollár_2025\",\r\n    \"Color\": {\r\n      \"A\": 255,\r\n      \"R\": 0,\r\n      \"G\": 0,\r\n      \"B\": 255\r\n    },\r\n    \"IsSelected\": false\r\n  },\r\n  {\r\n    \"Name\": \"Dollár_2026\",\r\n    \"Color\": {\r\n      \"A\": 255,\r\n      \"R\": 255,\r\n      \"G\": 0,\r\n      \"B\": 0\r\n    },\r\n    \"IsSelected\": false\r\n  },\r\n  {\r\n    \"Name\": \"Font_2025\",\r\n    \"Color\": {\r\n      \"A\": 255,\r\n      \"R\": 0,\r\n      \"G\": 128,\r\n      \"B\": 0\r\n    },\r\n    \"IsSelected\": false\r\n  },\r\n  {\r\n    \"Name\": \"Font_2026\",\r\n    \"Color\": {\r\n      \"A\": 255,\r\n      \"R\": 255,\r\n      \"G\": 0,\r\n      \"B\": 255\r\n    },\r\n    \"IsSelected\": false\r\n  }\r\n]', '91,92,93,94,95,96,397,398,399,400,401,402,403,404,405,406,407,408,409,410,411,412,413,414,415,416,417,418,419,420,421,422,423,424,425,426,427,428,429,430,431,432,433,434,435,436,437,438,439,440,441,442,443,444,445,446,447,448,449,450,451,452,453,454,455,456,457,458,459,460,461,462,463,464,465,466,467,468,469,470,471,472,473,474,475,476,477,478,479,480,481,482,483,484,485,486,487,488,489,490,491,492,493,494,495,496,617,618,619,620,621,622,623,624,625,626,627,628,629,630,631,632,633,634,635,636,637,638,639,640,641,642,643,644,645,646,647,648,649,650,651,652,653,654,655,656,657,658,659,660,661,662,663,664,665,666,667,668,669,670,671,672,673,674,675,676', 'Összeg', '2025-05-12 21:00:02', '2025-05-13 00:12:46', 1);

-- --------------------------------------------------------

--
-- Table structure for table `dolgozok`
--

CREATE TABLE `dolgozok` (
  `id` int(11) NOT NULL,
  `vezeteknev` varchar(30) NOT NULL,
  `keresztnev` varchar(30) NOT NULL,
  `email` varchar(60) NOT NULL,
  `telefonszam` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

--
-- Dumping data for table `dolgozok`
--

INSERT INTO `dolgozok` (`id`, `vezeteknev`, `keresztnev`, `email`, `telefonszam`) VALUES
(1, 'Lengyel', 'Norbert', 'lengyelnorbi5@gmail.com', '+36201234567'),
(45, 'Nagy', 'István', 'nagy.istvan@ceg.hu', '+36201234567'),
(46, 'Kovács', 'Éva', 'kovacs.eva@ceg.hu', '+36301234567'),
(47, 'Szabó', 'János', 'szabo.janos@ceg.hu', '+36701234567'),
(48, 'Tóth', 'Katalin', 'toth.katalin@ceg.hu', '+36202345678'),
(49, 'Kiss', 'Péter', 'kiss.peter@ceg.hu', '+36302345678'),
(50, 'Horváth', 'Anna', 'horvath.anna@ceg.hu', '+36702345678'),
(51, 'Varga', 'Zoltán', 'varga.zoltan@ceg.hu', '+36203456789'),
(52, 'Fehér', 'Mária', 'feher.maria@ceg.hu', '+36303456789'),
(53, 'Balázs', 'Gábor', 'balazs.gabor@ceg.hu', '+36703456789'),
(54, 'Németh', 'Júlia', 'nemeth.julia@ceg.hu', '+36204567890');

-- --------------------------------------------------------

--
-- Table structure for table `felhasznalok`
--

CREATE TABLE `felhasznalok` (
  `id` int(11) NOT NULL,
  `felhasznalo_nev` varchar(30) NOT NULL,
  `jelszo` varchar(64) NOT NULL,
  `dolgozo_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

--
-- Dumping data for table `felhasznalok`
--

INSERT INTO `felhasznalok` (`id`, `felhasznalo_nev`, `jelszo`, `dolgozo_id`) VALUES
(1, 'lennor', 'Asd12345', 1),
(2, 'teszter', 'Teszt123', 50),
(9, 'admin', 'admin123', 45),
(10, 'kovacse', 'jelszo123', 46),
(11, 'szaboj', 'jelszo456', 47),
(12, 'tothk', 'jelszo789', 48),
(13, 'kissp', 'jelszo012', 49);

-- --------------------------------------------------------

--
-- Table structure for table `gazdalkodo_szervezetek`
--

CREATE TABLE `gazdalkodo_szervezetek` (
  `id` int(11) NOT NULL,
  `nev` varchar(60) NOT NULL,
  `kapcsolattarto` varchar(100) NOT NULL,
  `email` varchar(60) NOT NULL,
  `telefonszam` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

--
-- Dumping data for table `gazdalkodo_szervezetek`
--

INSERT INTO `gazdalkodo_szervezetek` (`id`, `nev`, `kapcsolattarto`, `email`, `telefonszam`) VALUES
(9, 'ABC Kft.', 'Kovács János', 'info@abckft.hu', '+3612345678'),
(10, 'XYZ Zrt.', 'Nagy Péter', 'info@xyzzrt.hu', '+3613456789'),
(11, 'Tech Solutions Bt.', 'Szabó Márta', 'info@techsolutions.hu', '+3614567890'),
(12, 'DataSystems Kft.', 'Tóth Gábor', 'info@datasystems.hu', '+3615678901'),
(13, 'Global Partners Zrt.', 'Horváth Katalin', 'info@globalpartners.hu', '+3616789012');

-- --------------------------------------------------------

--
-- Table structure for table `kotelezettsegek_kovetelesek`
--

CREATE TABLE `kotelezettsegek_kovetelesek` (
  `id` int(11) NOT NULL,
  `tipus` varchar(20) NOT NULL,
  `osszeg` int(11) NOT NULL,
  `penznem` varchar(25) NOT NULL,
  `kifizetes_hatarideje` date NOT NULL,
  `kifizetett` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

--
-- Dumping data for table `kotelezettsegek_kovetelesek`
--

INSERT INTO `kotelezettsegek_kovetelesek` (`id`, `tipus`, `osszeg`, `penznem`, `kifizetes_hatarideje`, `kifizetett`) VALUES
(5, 'Ki1', 150000, 'Forint', '2025-06-15', 0),
(6, 'Ki2', 75000, 'Forint', '2025-06-30', 0),
(7, 'Be1', 300000, 'Forint', '2025-06-10', 1),
(8, 'Be2', 500, 'Euró', '2025-06-20', 0),
(9, 'Ki1', 95000, 'Forint', '2025-06-25', 1),
(10, 'Ki1', 185000, 'Forint', '2025-01-31', 1),
(11, 'Ki2', 320000, 'Forint', '2025-02-15', 1),
(12, 'Ki1', 90000, 'Forint', '2025-02-28', 1),
(13, 'Ki2', 220000, 'Forint', '2025-03-15', 1),
(14, 'Ki1', 130000, 'Forint', '2025-03-31', 1),
(15, 'Ki2', 380000, 'Forint', '2025-04-15', 1),
(16, 'Ki1', 110000, 'Forint', '2025-04-30', 0),
(17, 'Ki2', 290000, 'Forint', '2025-05-15', 0),
(18, 'Ki1', 150000, 'Forint', '2025-05-31', 0),
(19, 'Ki2', 310000, 'Forint', '2025-06-15', 0),
(20, 'Ki1', 95000, 'Forint', '2025-06-30', 0),
(21, 'Ki2', 265000, 'Forint', '2025-07-15', 0),
(22, 'Ki1', 170000, 'Forint', '2025-07-31', 0),
(23, 'Ki2', 345000, 'Forint', '2025-08-15', 0),
(24, 'Ki1', 120000, 'Forint', '2025-08-31', 0),
(25, 'Ki2', 255000, 'Forint', '2025-09-15', 0),
(26, 'Ki1', 140000, 'Forint', '2025-09-30', 0),
(27, 'Ki2', 375000, 'Forint', '2025-10-15', 0),
(28, 'Ki1', 105000, 'Forint', '2025-10-31', 0),
(29, 'Ki2', 285000, 'Forint', '2025-11-15', 0),
(30, 'Ki1', 165000, 'Forint', '2025-11-30', 0),
(31, 'Ki2', 405000, 'Forint', '2025-12-15', 0),
(32, 'Ki1', 115000, 'Forint', '2025-12-31', 0),
(33, 'Ki2', 295000, 'Forint', '2026-01-15', 0),
(34, 'Ki1', 180000, 'Forint', '2026-01-31', 0),
(35, 'Ki2', 435000, 'Forint', '2026-02-15', 0),
(36, 'Ki1', 125000, 'Forint', '2026-02-28', 0),
(37, 'Ki2', 315000, 'Forint', '2026-03-15', 0),
(38, 'Ki1', 195000, 'Forint', '2026-03-31', 0),
(39, 'Ki2', 355000, 'Forint', '2026-04-15', 0),
(40, 'Ki1', 135000, 'Forint', '2026-04-30', 0),
(41, 'Ki2', 335000, 'Forint', '2026-05-15', 0),
(42, 'Ki1', 205000, 'Forint', '2026-05-31', 0),
(43, 'Ki2', 375000, 'Forint', '2026-06-15', 0),
(44, 'Ki1', 145000, 'Forint', '2026-06-30', 0),
(45, 'Ki2', 355000, 'Forint', '2026-07-15', 0),
(46, 'Ki1', 215000, 'Forint', '2026-07-31', 0),
(47, 'Ki2', 395000, 'Forint', '2026-08-15', 0),
(48, 'Ki1', 155000, 'Forint', '2026-08-31', 0),
(49, 'Ki2', 375000, 'Forint', '2026-09-15', 0),
(50, 'Be1', 250000, 'Forint', '2025-01-15', 1),
(51, 'Be2', 500000, 'Forint', '2025-01-30', 1),
(52, 'Be1', 1200, 'Euró', '2025-02-15', 1),
(53, 'Be1', 175000, 'Forint', '2025-02-28', 1),
(54, 'Be2', 450000, 'Forint', '2025-03-15', 1),
(55, 'Be1', 800, 'Euró', '2025-03-31', 1),
(56, 'Be1', 275000, 'Forint', '2025-04-15', 1),
(57, 'Be2', 550000, 'Forint', '2025-04-30', 1),
(58, 'Be1', 950, 'Euró', '2025-05-15', 0),
(59, 'Be1', 225000, 'Forint', '2025-05-31', 0),
(60, 'Be2', 600000, 'Forint', '2025-06-15', 0),
(61, 'Be1', 1050, 'Euró', '2025-06-30', 0),
(62, 'Be1', 335000, 'Forint', '2025-07-15', 0),
(63, 'Be2', 520000, 'Forint', '2025-07-31', 0),
(64, 'Be1', 750, 'Euró', '2025-08-15', 0),
(65, 'Be1', 285000, 'Forint', '2025-08-31', 0),
(66, 'Be2', 650000, 'Forint', '2025-09-15', 0),
(67, 'Be1', 1100, 'Euró', '2025-09-30', 0),
(68, 'Be1', 305000, 'Forint', '2025-10-15', 0),
(69, 'Be2', 580000, 'Forint', '2025-10-31', 0),
(70, 'Be1', 850, 'Euró', '2025-11-15', 0),
(71, 'Be1', 215000, 'Forint', '2025-11-30', 0),
(72, 'Be2', 700000, 'Forint', '2025-12-15', 0),
(73, 'Be1', 1150, 'Euró', '2025-12-31', 0),
(74, 'Be1', 365000, 'Forint', '2026-01-15', 0),
(75, 'Be2', 620000, 'Forint', '2026-01-31', 0),
(76, 'Be1', 900, 'Euró', '2026-02-15', 0),
(77, 'Be1', 235000, 'Forint', '2026-02-28', 0),
(78, 'Be2', 750000, 'Forint', '2026-03-15', 0),
(79, 'Be1', 1250, 'Euró', '2026-03-31', 0),
(80, 'Be1', 245000, 'Forint', '2026-04-15', 0),
(81, 'Be2', 660000, 'Forint', '2026-04-30', 0),
(82, 'Be1', 950, 'Euró', '2026-05-15', 0),
(83, 'Be1', 255000, 'Forint', '2026-05-31', 0),
(84, 'Be2', 800000, 'Forint', '2026-06-15', 0),
(85, 'Be1', 1300, 'Euró', '2026-06-30', 0),
(86, 'Be1', 425000, 'Forint', '2026-07-15', 0),
(87, 'Be2', 700000, 'Forint', '2026-07-31', 0),
(88, 'Be1', 1000, 'Euró', '2026-08-15', 0),
(89, 'Be1', 275000, 'Forint', '2026-08-31', 0),
(90, 'Be2', 850000, 'Forint', '2026-09-15', 0),
(91, 'Be1', 1350, 'Euró', '2026-09-30', 0),
(92, 'Be1', 465000, 'Forint', '2026-10-15', 0),
(93, 'Be2', 740000, 'Forint', '2026-10-31', 0),
(94, 'Be1', 1050, 'Euró', '2026-11-15', 0),
(95, 'Be1', 295000, 'Forint', '2026-11-30', 0),
(96, 'Be2', 900000, 'Forint', '2026-12-15', 0),
(97, 'Be1', 1400, 'Euró', '2026-12-31', 0),
(98, 'Be1', 485000, 'Forint', '2027-01-15', 0),
(99, 'Be2', 780000, 'Forint', '2027-01-31', 0),
(100, 'Be1', 1100, 'Euró', '2027-02-15', 0),
(101, 'Be1', 315000, 'Forint', '2027-02-28', 0),
(102, 'Be2', 950000, 'Forint', '2027-03-15', 0),
(103, 'Be1', 1450, 'Euró', '2027-03-31', 0),
(104, 'Be1', 505000, 'Forint', '2027-04-15', 0),
(105, 'Be2', 820000, 'Forint', '2027-04-30', 0),
(106, 'Be1', 1150, 'Euró', '2027-05-15', 0),
(107, 'Be1', 335000, 'Forint', '2027-05-31', 0),
(108, 'Be2', 1000000, 'Forint', '2027-06-15', 0),
(109, 'Be1', 1500, 'Euró', '2027-06-30', 0),
(110, 'Be1', 1800, 'Dollár', '2025-06-20', 0),
(111, 'Be2', 3500, 'Dollár', '2025-07-05', 0),
(112, 'Ki1', 1400, 'Dollár', '2025-07-20', 0),
(113, 'Ki2', 4800, 'Dollár', '2025-08-05', 0),
(114, 'Be1', 2200, 'Dollár', '2025-08-20', 0),
(115, 'Ki1', 1750, 'Dollár', '2025-09-05', 0),
(116, 'Be2', 5200, 'Dollár', '2025-09-20', 0),
(117, 'Ki2', 2400, 'Dollár', '2025-10-05', 0),
(118, 'Be1', 1850, 'Dollár', '2025-10-20', 0),
(119, 'Be2', 3800, 'Dollár', '2025-11-05', 0),
(120, 'Ki1', 2950, 'Dollár', '2025-11-20', 0),
(121, 'Be1', 1650, 'Dollár', '2025-12-05', 0),
(122, 'Ki2', 4250, 'Dollár', '2025-12-20', 0),
(123, 'Be2', 3300, 'Dollár', '2026-01-05', 1),
(124, 'Ki1', 1900, 'Dollár', '2026-01-20', 1),
(125, 'Be1', 2750, 'Dollár', '2026-02-05', 1),
(126, 'Ki2', 4900, 'Dollár', '2026-02-20', 1),
(127, 'Be2', 2150, 'Dollár', '2026-03-05', 1),
(128, 'Ki1', 3650, 'Dollár', '2026-03-20', 1),
(129, 'Be1', 1950, 'Dollár', '2026-04-05', 1),
(130, 'Ki2', 5300, 'Dollár', '2026-04-20', 0),
(131, 'Be2', 2850, 'Dollár', '2026-05-05', 0),
(132, 'Ki1', 1700, 'Dollár', '2026-05-20', 0),
(133, 'Be1', 4600, 'Dollár', '2026-06-05', 0),
(134, 'Ki2', 3100, 'Dollár', '2026-06-20', 0),
(135, 'Be2', 2050, 'Dollár', '2026-07-05', 0),
(136, 'Ki1', 5100, 'Dollár', '2026-07-20', 0),
(137, 'Be1', 3750, 'Dollár', '2026-08-05', 0),
(138, 'Ki2', 1550, 'Dollár', '2026-08-20', 0),
(139, 'Be2', 4100, 'Dollár', '2026-09-05', 0),
(140, 'Be1', 1500, 'Font', '2025-06-25', 0),
(141, 'Be2', 2700, 'Font', '2025-07-10', 0),
(142, 'Ki1', 1300, 'Font', '2025-07-25', 0),
(143, 'Ki2', 3500, 'Font', '2025-08-10', 0),
(144, 'Be1', 1900, 'Font', '2025-08-25', 0),
(145, 'Ki1', 1400, 'Font', '2025-09-10', 0),
(146, 'Be2', 4100, 'Font', '2025-09-25', 0),
(147, 'Ki2', 2000, 'Font', '2025-10-10', 0),
(148, 'Be1', 1600, 'Font', '2025-10-25', 0),
(149, 'Be2', 3200, 'Font', '2025-11-10', 0),
(150, 'Ki1', 2500, 'Font', '2025-11-25', 0),
(151, 'Be1', 1450, 'Font', '2025-12-10', 0),
(152, 'Ki2', 3800, 'Font', '2025-12-25', 0),
(153, 'Be2', 2900, 'Font', '2026-01-10', 1),
(154, 'Ki1', 1650, 'Font', '2026-01-25', 1),
(155, 'Be1', 2300, 'Font', '2026-02-10', 1),
(156, 'Ki2', 4300, 'Font', '2026-02-25', 1),
(157, 'Be2', 1850, 'Font', '2026-03-10', 1),
(158, 'Ki1', 3100, 'Font', '2026-03-25', 1),
(159, 'Be1', 1750, 'Font', '2026-04-10', 1),
(160, 'Ki2', 4500, 'Font', '2026-04-25', 0),
(161, 'Be2', 2400, 'Font', '2026-05-10', 0),
(162, 'Ki1', 1350, 'Font', '2026-05-25', 0),
(163, 'Be1', 3900, 'Font', '2026-06-10', 0),
(164, 'Ki2', 2800, 'Font', '2026-06-25', 0),
(165, 'Be2', 1700, 'Font', '2026-07-10', 0),
(166, 'Ki1', 4200, 'Font', '2026-07-25', 0),
(167, 'Be1', 3300, 'Font', '2026-08-10', 0),
(168, 'Ki2', 1250, 'Font', '2026-08-25', 0),
(169, 'Be2', 3600, 'Font', '2026-09-10', 0);

-- --------------------------------------------------------

--
-- Table structure for table `magan_szemelyek`
--

CREATE TABLE `magan_szemelyek` (
  `id` int(11) NOT NULL,
  `nev` varchar(50) NOT NULL,
  `telefonszam` varchar(15) NOT NULL,
  `email` varchar(50) NOT NULL,
  `lakcim` varchar(60) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

--
-- Dumping data for table `magan_szemelyek`
--

INSERT INTO `magan_szemelyek` (`id`, `nev`, `telefonszam`, `email`, `lakcim`) VALUES
(8, 'Szőke Mária', '+36304567891', 'szoke.maria@email.com', 'Debrecen, Petőfi tér 5.'),
(9, 'Lakatos Gergő', '+36704567891', 'lakatos.gergo@email.com', 'Szeged, Dózsa György út 8.'),
(10, 'Bíró Katalin', '+36204567892', 'biro.katalin@email.com', 'Pécs, Rákóczi út 15.'),
(11, 'Molnár Attila', '+36304567892', 'molnar.attila@email.com', 'Győr, Árpád út 22.'),
(12, 'Lengyel Norbert', '06201234567', 'lengyelnorbi5@gmail.com', 'Szeged'),
(13, 'Fekete Pál', '+36204567891', 'fekete.pal@email.com', 'Budapest, Kossuth utca 10.');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `bevetelek_kiadasok`
--
ALTER TABLE `bevetelek_kiadasok`
  ADD PRIMARY KEY (`id`),
  ADD KEY `kotel_kovet_id` (`kotel_kovet_id`),
  ADD KEY `parner_id` (`gazdalkodo_szerv_id`),
  ADD KEY `magan_szemely_id` (`magan_szemely_id`);

--
-- Indexes for table `diagrammok`
--
ALTER TABLE `diagrammok`
  ADD PRIMARY KEY (`id`),
  ADD KEY `letrehozo_id` (`letrehozo_id`);

--
-- Indexes for table `dolgozok`
--
ALTER TABLE `dolgozok`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `felhasznalok`
--
ALTER TABLE `felhasznalok`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_dolgozo_id` (`dolgozo_id`);

--
-- Indexes for table `gazdalkodo_szervezetek`
--
ALTER TABLE `gazdalkodo_szervezetek`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `kotelezettsegek_kovetelesek`
--
ALTER TABLE `kotelezettsegek_kovetelesek`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `magan_szemelyek`
--
ALTER TABLE `magan_szemelyek`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `bevetelek_kiadasok`
--
ALTER TABLE `bevetelek_kiadasok`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=677;

--
-- AUTO_INCREMENT for table `diagrammok`
--
ALTER TABLE `diagrammok`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=36;

--
-- AUTO_INCREMENT for table `dolgozok`
--
ALTER TABLE `dolgozok`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=55;

--
-- AUTO_INCREMENT for table `felhasznalok`
--
ALTER TABLE `felhasznalok`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=14;

--
-- AUTO_INCREMENT for table `gazdalkodo_szervezetek`
--
ALTER TABLE `gazdalkodo_szervezetek`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=14;

--
-- AUTO_INCREMENT for table `kotelezettsegek_kovetelesek`
--
ALTER TABLE `kotelezettsegek_kovetelesek`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=170;

--
-- AUTO_INCREMENT for table `magan_szemelyek`
--
ALTER TABLE `magan_szemelyek`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=14;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `bevetelek_kiadasok`
--
ALTER TABLE `bevetelek_kiadasok`
  ADD CONSTRAINT `bevetelek_kiadasok_ibfk_1` FOREIGN KEY (`kotel_kovet_id`) REFERENCES `kotelezettsegek_kovetelesek` (`id`),
  ADD CONSTRAINT `bevetelek_kiadasok_ibfk_4` FOREIGN KEY (`gazdalkodo_szerv_id`) REFERENCES `gazdalkodo_szervezetek` (`id`),
  ADD CONSTRAINT `bevetelek_kiadasok_ibfk_5` FOREIGN KEY (`magan_szemely_id`) REFERENCES `magan_szemelyek` (`id`);

--
-- Constraints for table `diagrammok`
--
ALTER TABLE `diagrammok`
  ADD CONSTRAINT `diagrammok_ibfk_1` FOREIGN KEY (`letrehozo_id`) REFERENCES `felhasznalok` (`id`);

--
-- Constraints for table `felhasznalok`
--
ALTER TABLE `felhasznalok`
  ADD CONSTRAINT `fk_dolgozo_id` FOREIGN KEY (`dolgozo_id`) REFERENCES `dolgozok` (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
