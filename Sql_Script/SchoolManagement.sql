CREATE TABLE Student (
    ID NUMBER GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    First_Name VARCHAR2(255) NOT NULL,
    Last_Name VARCHAR2(255) NOT NULL,
    Gender VARCHAR2(50) NOT NULL,
    ContactNumber VARCHAR2(255) NOT NULL,
    DOB DATE NOT NULL,
    Email VARCHAR2(255),
    Profile_Picture Blob,
    Status NUMBER(1) DEFAULT 1 NOT NULL
);

CREATE TABLE family_background (
    ID NUMBER GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    student_id NUMBER NOT NULL,
    Father_Name VARCHAR2(255),
    Father_Occupation VARCHAR2(255),
    Mother_Name VARCHAR2(255),
    Mother_Occupation VARCHAR2(255),
    contact_number VARCHAR2(255),
    Current_Address VARCHAR2(255),
    CONSTRAINT fk_student_id FOREIGN KEY (student_id) REFERENCES Student (ID)
)
/
CREATE TABLE user_tbl (
   ID NUMBER GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
   Username VARCHAR2(255),
   Password VARCHAR2(255);
    FIRST_NAME VARCHAR2(50 BYTE),
    LAST_NAME VARCHAR2(50 BYTE),
    CREATE_AT TIMESTAMP(6),
    UPDATE_AT TIMESTAMP(6),
    CONSTRAINT pk_user_tbl PRIMARY KEY (ID)
);
/
CREATE TABLE Subject (
    Subject_ID NUMBER GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    Name VARCHAR2(255) NOT NULL,
    Description VARCHAR2(1000)
);
/
CREATE TABLE Scores (
    Score_ID NUMBER GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    Student_ID NUMBER NOT NULL,
    Subject_ID NUMBER NOT NULL,
    Year NUMBER NOT NULL CHECK (Year BETWEEN 1 AND 4),
    Semester NUMBER NOT NULL CHECK (Semester IN (1, 2)),
    Score NUMBER CHECK (Score BETWEEN 0 AND 100),
    FOREIGN KEY (Student_ID) REFERENCES Student(ID),
    FOREIGN KEY (Subject_ID) REFERENCES Subject(Subject_ID)
);
/
CREATE INDEX idx_scores_student ON Scores(Student_ID);
CREATE INDEX idx_scores_subject ON Scores(Subject_ID);
CREATE INDEX idx_scores_year_semester ON Scores(Year, Semester);
/
CREATE OR REPLACE PROCEDURE InsertSubject(
    p_subject_name VARCHAR2,
    p_subject_desc VARCHAR2)
AS
BEGIN
    INSERT INTO Subject (Name, Description)
    VALUES (p_subject_name, p_subject_desc);
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error: ' || SQLERRM);
END;
/
CREATE OR REPLACE PROCEDURE InsertScore(
    p_student_id NUMBER,
    p_subject_id NUMBER,
    p_year NUMBER,
    p_semester NUMBER,
    p_score NUMBER)
AS
BEGIN
    INSERT INTO Scores (Student_ID, Subject_ID, Year, Semester, Score)
    VALUES (p_student_id, p_subject_id, p_year, p_semester, p_score);
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error: ' || SQLERRM);
END;
/
CREATE OR REPLACE PROCEDURE UpdateScore(
    p_student_id NUMBER,
    p_subject_id NUMBER,
    p_year NUMBER,
    p_semester NUMBER,
    p__score NUMBER)
AS
BEGIN
    -- Updating scores
    UPDATE Scores
    SET Score = p_score
    WHERE Student_ID = p_student_id
      AND Subject_ID = p_subject_id
      AND Year = p_year
      AND Semester = p_semester;

    -- Commit the changes
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN
        -- Roll back if there is an error
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error: ' || SQLERRM);
END;
/
CREATE OR REPLACE PROCEDURE CreateStudent (
    p_First_Name IN Student.First_Name%TYPE,
    p_Last_Name IN Student.Last_Name%TYPE,
    p_Gender IN Student.Gender%TYPE,
    p_ContactNumber IN Student.ContactNumber%TYPE,
    p_DOB IN Student.DOB%TYPE,
    p_Email IN Student.Email%TYPE,
    p_Profile_Picture IN Student.Profile_Picture%TYPE,
    p_Status IN Student.Status%TYPE
     p_id out Student.Id%TYPE
) AS
BEGIN
    INSERT INTO Student (First_Name, Last_Name, Gender, ContactNumber, DOB, Email, Profile_Picture, Status)
    VALUES (p_First_Name, p_Last_Name, p_Gender, p_ContactNumber, p_DOB, p_Email, p_Profile_Picture, p_Status)
    Returning ID into p_id;
END;
/

CREATE OR REPLACE PROCEDURE UpdateStudent (
    p_ID IN Student.ID%TYPE,
    p_First_Name IN Student.First_Name%TYPE,
    p_Last_Name IN Student.Last_Name%TYPE,
    p_Gender IN Student.Gender%TYPE,
    p_ContactNumber IN Student.ContactNumber%TYPE,
    p_DOB IN Student.DOB%TYPE,
    p_Email IN Student.Email%TYPE,
    p_Profile_Picture IN Student.Profile_Picture%TYPE,
    p_Status IN Student.Status%TYPE
) AS
BEGIN
    UPDATE Student
    SET First_Name = p_First_Name,
        Last_Name = p_Last_Name,
        Gender = p_Gender,
        ContactNumber = p_ContactNumber,
        DOB = p_DOB,
        Email = p_Email,
        Profile_Picture = p_Profile_Picture,
        Status = p_Status
    WHERE ID = p_ID;
END;
/
CREATE OR REPLACE PROCEDURE DeleteStudent (p_ID IN Student.ID%TYPE) AS
BEGIN
    DELETE FROM Student WHERE ID = p_ID;
END;
/
CREATE OR REPLACE PROCEDURE UpdateStudentStatus (
    p_ID IN Student.ID%TYPE,
    p_Status IN Student.Status%TYPE
) AS
BEGIN
    UPDATE Student
    SET Status = p_Status
    WHERE ID = p_ID;
END;
/
CREATE OR REPLACE PROCEDURE CreateFamilyBackground (
    p_student_id IN family_background.student_id%TYPE,
    p_Father_Name IN family_background.Father_Name%TYPE,
    p_Father_Occupation IN family_background.Father_Occupation%TYPE,
    p_Mother_Name IN family_background.Mother_Name%TYPE,
    p_Mother_Occupation IN family_background.Mother_Occupation%TYPE,
    p_contact_number IN family_background.contact_number%TYPE,
    p_Current_Address IN family_background.Current_Address%TYPE
) AS
BEGIN
    INSERT INTO family_background (student_id, Father_Name, Father_Occupation, Mother_Name, Mother_Occupation, contact_number, Current_Address)
    VALUES (p_student_id, p_Father_Name, p_Father_Occupation, p_Mother_Name, p_Mother_Occupation, p_contact_number, p_Current_Address);
END;
/
CREATE OR REPLACE PROCEDURE UpdateFamilyBackground (
    p_student_id IN family_background.student_id%TYPE,
    p_Father_Name IN family_background.Father_Name%TYPE,
    p_Father_Occupation IN family_background.Father_Occupation%TYPE,
    p_Mother_Name IN family_background.Mother_Name%TYPE,
    p_Mother_Occupation IN family_background.Mother_Occupation%TYPE,
    p_contact_number IN family_background.contact_number%TYPE,
    p_Current_Address IN family_background.Current_Address%TYPE
) AS
BEGIN
    UPDATE family_background
    SET Father_Name = p_Father_Name,
        Father_Occupation = p_Father_Occupation,
        Mother_Name = p_Mother_Name,
        Mother_Occupation = p_Mother_Occupation,
        contact_number = p_contact_number,
        Current_Address = p_Current_Address
    WHERE student_id = p_student_id;
END;
/
CREATE OR REPLACE PROCEDURE DeleteFamilyBackground (p_ID IN family_background.ID%TYPE) AS
BEGIN
    DELETE FROM family_background WHERE ID = p_ID;
END;
/
CREATE OR REPLACE PROCEDURE addUser(
    p_username IN user_tbl.Username%TYPE,
    p_password IN user_tbl.Password%TYPE,
    p_first_name IN user_tbl.FIRST_NAME%TYPE,
    p_last_name IN user_tbl.LAST_NAME%TYPE,
    p_create_at IN user_tbl.CREATE_AT%TYPE,
    p_update_at IN user_tbl.UPDATE_AT%TYPE
)
AS
BEGIN
    INSERT INTO user_tbl (Username, Password, FIRST_NAME, LAST_NAME, CREATE_AT, UPDATE_AT)
    VALUES (p_username, p_password, p_first_name, p_last_name, SYSTIMESTAMP, SYSTIMESTAMP);
END;
/
CREATE OR REPLACE PROCEDURE updateUser(
    p_id IN user_tbl.ID%TYPE,
    p_username IN user_tbl.Username%TYPE,
    p_password IN user_tbl.Password%TYPE,
    p_first_name IN user_tbl.FIRST_NAME%TYPE,
    p_last_name IN user_tbl.LAST_NAME%TYPE
)
AS
BEGIN
    UPDATE user_tbl
    SET Username = p_username,
        Password = p_password,
        FIRST_NAME = p_first_name,
        LAST_NAME = p_last_name,
        UPDATE_AT = SYSTIMESTAMP
    WHERE ID = p_id;
END;
/
CREATE OR REPLACE PROCEDURE updatePassword(
    p_id IN user_tbl.ID%TYPE,
    p_new_password IN user_tbl.Password%TYPE
)
AS
BEGIN
    UPDATE user_tbl
    SET Password = p_new_password, UPDATE_AT = SYSTIMESTAMP
    WHERE ID = p_id;
END;
/