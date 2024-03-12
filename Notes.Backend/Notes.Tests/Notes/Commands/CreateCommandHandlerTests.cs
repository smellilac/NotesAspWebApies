using Microsoft.EntityFrameworkCore;
using Notes.Application.Commands.CreateNote;
using Notes.Tests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Tests.Notes.Commands;

public class CreateCommandHandlerTests : TestCommandBase
{
    [Fact]
    public async Task CreateNoteCommandHandler_Succes()
    {
        // Arrange 
        var handler = new CreateNoteCommandHandler(context);
        var noteName = " note name ";
        var noteDetails = "note details";
        //Act

        var noteId = await handler.Handle(
            new CreateNoteCommand
            {
                Title = noteName,
                Details = noteDetails,
                UserId = NotesContextFactory.UserAId
            },
            CancellationToken.None);

        // Assert
        Assert.NotNull(
                await context.Notes.SingleOrDefaultAsync(note =>
                    note.Id == noteId && note.Title == noteName &&
                    note.Details == noteDetails));
    }
}
