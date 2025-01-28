public interface IClassService
{
    Task<Class> CreateClassAsync(ClassDTO classDTO);
    Task<List<Class>> GetAllClassesAsync();
    Task<Class> GetClassAsync(int id);
    Task<Class> UpdateClassAsync(int id, ClassDTO classDTO);
    Task<bool> DeleteClassAsync(int id);
}