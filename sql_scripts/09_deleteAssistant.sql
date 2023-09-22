-- Check if the assistant exists
IF EXISTS (SELECT 1 FROM Assistant WHERE Name = 'Robin')

BEGIN
    -- Delete the assistant
    DELETE FROM Assistant WHERE Name = 'Robin';
    PRINT 'Assistant deleted successfully.';
END
ELSE
BEGIN
    PRINT 'Assistant with the specified name does not exist.';
END


