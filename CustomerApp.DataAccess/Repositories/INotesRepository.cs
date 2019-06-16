namespace CustomerApp.DataAccess.Repositories
{
    using System.Threading.Tasks;
    using Models;

    public interface INotesRepository
    {
        Task<Result> UpdateNote(NoteDetail noteDetail);

        Task<NoteDetail> AddNote(NewNote newNote);
    }
}