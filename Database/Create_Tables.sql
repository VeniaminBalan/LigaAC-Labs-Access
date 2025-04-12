-- Switch to the newly created database
USE Quiz;
GO

-- Create the Questions table
CREATE TABLE Questions (
    ID INT IDENTITY(1,1) PRIMARY KEY,  -- Auto-incrementing ID as primary key
    QuestionText NVARCHAR(255) NOT NULL  -- The text of the question
);
GO

-- Create the Answers table
CREATE TABLE Answers (
    ID INT IDENTITY(1,1) PRIMARY KEY,  -- Auto-incrementing ID as primary key
    AnswerText NVARCHAR(255) NOT NULL,  -- The text of the answer
    Correct BIT NOT NULL,              -- Whether the answer is correct or not
    QuestionId INT,                    -- Foreign key reference to the Questions table
    CONSTRAINT FK_Answers_Questions FOREIGN KEY (QuestionId) REFERENCES Questions(ID)
);
GO

-- Optional: Add some sample data to the tables

-- Insert sample questions
INSERT INTO Questions (QuestionText) VALUES 
('What is the capital of France?'),
('Who developed the theory of relativity?'),
('What is the largest planet in our solar system?');

-- Insert sample answers
INSERT INTO Answers (AnswerText, Correct, QuestionId) VALUES
('Paris', 1, 1),
('London', 0, 1),
('Einstein', 1, 2),
('Newton', 0, 2),
('Jupiter', 1, 3),
('Saturn', 0, 3);
GO

-- Verify the data
SELECT * FROM Questions;
SELECT * FROM Answers;
