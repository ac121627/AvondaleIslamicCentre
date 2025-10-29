SELECT s2.FirstName, s2.LastName
FROM Students s1
JOIN Students s2 ON s1.TeacherId = s2.TeacherId
WHERE s1.FirstName = 'Ayaan' AND s1.LastName = 'Khan' AND s2.StudentId != s1.StudentId;
