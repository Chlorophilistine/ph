namespace CustomerApp.DataAccess.Repositories
{
    using System.ComponentModel.Composition;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Threading.Tasks;
    using Models;

    [Export(typeof(INotesRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class NotesRepository : INotesRepository
    {
        private readonly ICustomerContext _context;

        [ImportingConstructor]
        public NotesRepository(ICustomerContext context)
        {
            _context = context;
        }

        public async Task<Result> UpdateNote(NoteDetail noteDetail)
        {
            var note = await _context.Notes.FindAsync(noteDetail.Id);

            if (note is null)
            {
                return Result.NotFound;
            }

            note.Content = noteDetail.Content;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NoteExists(noteDetail.Id))
                {
                    return Result.NotFound;
                }

                throw;
            }

            return Result.Completed;
        }

        public async Task<NoteDetail> AddNote(NewNote newNote)
        {
            var note = newNote.ToEntity();

            _context.Notes.Add(note);
            await _context.SaveChangesAsync();

            return note.ToModel();
        }

        private bool NoteExists(int id)
        {
            return _context.Notes.Count(e => e.Id == id) > 0;
        }
    }
}