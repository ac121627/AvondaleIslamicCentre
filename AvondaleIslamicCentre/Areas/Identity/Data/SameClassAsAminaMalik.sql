SELECT s2.FirstName, s2.LastName
FROM Students s1
JOIN Students s2 ON s1.ClassId = s2.ClassId
WHERE s1.FirstName = 'Amina' AND s1.LastName = 'Malik' AND s2.StudentId != s1.StudentId;