using Microsoft.EntityFrameworkCore;
using Schoolproject.Data;

public class ClassService : IClassService
{
    private readonly SchoolContext _context;

    public ClassService(SchoolContext context)
    {
        _context = context;
    }

    public async Task<Class> CreateClassAsync(ClassDTO classDTO)
    {
        var newClass = new Class { Name = classDTO.Name };
        _context.Classes.Add(newClass);
        await _context.SaveChangesAsync();
        return newClass;
    }

    public async Task<Class> GetClassAsync(int id)
    {
        return await _context.Classes
            .Include(c => c.Students)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Class?> UpdateClassAsync(int id, ClassDTO classDTO)
    {
        // Check if the class exists in the database
        var existingClass = await _context.Classes.FindAsync(id);
        if (existingClass == null)
        {
            // Return null or throw an exception to indicate the record was not found
            return null;
        }

        // Update the properties of the existing class
        existingClass.Name = classDTO.Name;

        // Save changes to the database
        _context.Classes.Update(existingClass);
        await _context.SaveChangesAsync();

        return existingClass;
    }


    public async Task<bool> DeleteClassAsync(int id)
    {
        var classToDelete = await _context.Classes.FindAsync(id);
        if (classToDelete == null)
        {
            return false;
        }

        _context.Classes.Remove(classToDelete);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Class>> GetAllClassesAsync()
    {
        return await _context.Classes
            .Include(c => c.Students)
            .ToListAsync();
    }
}
