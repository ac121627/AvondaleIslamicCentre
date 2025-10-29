SELECT s2.FirstName, s2.LastName, s2.Ethnicity
FROM Students s1
JOIN Students s2 ON s1.Ethnicity = s2.Ethnicity
WHERE s1.FirstName = 'Hana' AND s1.LastName = 'Ali' 
    AND s2.StudentId != s1.StudentId;