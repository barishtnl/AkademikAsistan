-- =====================================================================
-- Akademik Asistan ve Gelişim Takip Uygulaması — SQLite Şema
-- =====================================================================
PRAGMA foreign_keys = ON;

-- ---------------------------------------------------------------------
-- MODÜL 1: Ders Planı
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS Courses (
    CourseId        INTEGER PRIMARY KEY AUTOINCREMENT,
    CourseName      TEXT NOT NULL,
    CourseCode      TEXT,
    InstructorName  TEXT,
    DayOfWeek       INTEGER NOT NULL CHECK (DayOfWeek BETWEEN 1 AND 7), -- 1=Pazartesi ... 7=Pazar
    StartTime       TEXT NOT NULL,   -- 'HH:mm'
    EndTime         TEXT NOT NULL,   -- 'HH:mm'
    Classroom       TEXT,
    Credit          INTEGER NOT NULL DEFAULT 0,
    Semester        TEXT NOT NULL,   -- Örn: '2025-2026 Güz'
    CreatedAt       TEXT NOT NULL DEFAULT (datetime('now'))
);

CREATE INDEX IF NOT EXISTS IX_Courses_Semester ON Courses (Semester);

-- ---------------------------------------------------------------------
-- MODÜL 2: Not Hesaplama
-- ---------------------------------------------------------------------

-- Bir derse ait sınav/ödev bileşenleri (vize, final, ödev, proje vb.)
CREATE TABLE IF NOT EXISTS GradeComponents (
    GradeComponentId INTEGER PRIMARY KEY AUTOINCREMENT,
    CourseId         INTEGER NOT NULL,
    ComponentName    TEXT NOT NULL,      -- 'Vize', 'Final', 'Ödev' vb.
    Score            REAL,
    WeightPercentage REAL NOT NULL,
    FOREIGN KEY (CourseId) REFERENCES Courses (CourseId) ON DELETE CASCADE
);

-- Hesaplanan ders sonuçları (geçmişe dönük kayıt için)
CREATE TABLE IF NOT EXISTS CourseResults (
    CourseResultId  INTEGER PRIMARY KEY AUTOINCREMENT,
    CourseId        INTEGER NOT NULL,
    WeightedScore   REAL NOT NULL,
    LetterGrade     TEXT NOT NULL,
    GradePoint      REAL NOT NULL,
    CalculatedAt    TEXT NOT NULL DEFAULT (datetime('now')),
    FOREIGN KEY (CourseId) REFERENCES Courses (CourseId) ON DELETE CASCADE
);

-- Harf notu aralıkları — birden fazla sistem (Standart/Mutlak, Bağıl vb.) tutulabilir.
-- ÖNEMLİ: Buradaki değerler örnek/placeholder'dır; Gazi Üniversitesi'nin
-- güncel resmi tablosuyla (veya senin konsol uygulamandaki değerlerle) değiştirilmelidir.
CREATE TABLE IF NOT EXISTS LetterGradeRanges (
    LetterGradeRangeId INTEGER PRIMARY KEY AUTOINCREMENT,
    GradingSystemName  TEXT NOT NULL,   -- 'Standart' / 'Bagil'
    LetterGrade        TEXT NOT NULL,
    MinScore            REAL NOT NULL,
    MaxScore            REAL NOT NULL,
    GradePoint          REAL NOT NULL
);

-- ---------------------------------------------------------------------
-- MODÜL 3: Öğrenci Gelişim Takibi (GNO / YNO)
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS SemesterRecords (
    SemesterRecordId INTEGER PRIMARY KEY AUTOINCREMENT,
    SemesterName     TEXT NOT NULL,     -- '2024-2025 Güz'
    SemesterGpa      REAL NOT NULL,     -- DNO (o dönemin ağırlıklı ortalaması)
    CumulativeGpa    REAL NOT NULL,     -- GNO (genel ağırlıklı ortalama)
    TotalCredits     INTEGER NOT NULL,
    RecordedAt       TEXT NOT NULL DEFAULT (datetime('now'))
);

CREATE UNIQUE INDEX IF NOT EXISTS UX_SemesterRecords_SemesterName ON SemesterRecords (SemesterName);

-- ---------------------------------------------------------------------
-- Örnek seed veri: Standart (mutlak) harf notu aralıkları
-- ---------------------------------------------------------------------
INSERT INTO LetterGradeRanges (GradingSystemName, LetterGrade, MinScore, MaxScore, GradePoint) VALUES
    ('Standart', 'AA', 90, 100, 4.00),
    ('Standart', 'BA', 85, 89.99, 3.50),
    ('Standart', 'BB', 80, 84.99, 3.00),
    ('Standart', 'CB', 75, 79.99, 2.50),
    ('Standart', 'CC', 70, 74.99, 2.00),
    ('Standart', 'DC', 65, 69.99, 1.50),
    ('Standart', 'DD', 60, 64.99, 1.00),
    ('Standart', 'FD', 50, 59.99, 0.50),
    ('Standart', 'FF', 0,  49.99, 0.00);
