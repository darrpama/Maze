using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;
using GuiInterface.ViewModels;
using ReactiveUI;

namespace GuiInterface.Views;

public partial class MazeView : ReactiveUserControl<MazeViewModel>
{
    public MazeView()
    {
        InitializeComponent();
        this.WhenActivated(d =>
        {
            d(ViewModel!.ImportMazeInteraction.RegisterHandler(ImportMazeInteractionHandler));
            d(ViewModel!.ExportMazeInteraction.RegisterHandler(ExportMazeInteractionHandler));
        });
    }


    private async Task ImportMazeInteractionHandler(InteractionContext<string?, string?> context)
    {
        var topLevel = TopLevel.GetTopLevel(this);

        var storageFiles = await topLevel.StorageProvider
            .OpenFilePickerAsync(
                new FilePickerOpenOptions
                {
                    Title = "Open Maze File",
                    AllowMultiple = false,
                    FileTypeFilter = new[]
                    {
                        new FilePickerFileType("txt")
                        {
                            Patterns = new[]
                            {
                                "*.txt",
                            }
                        },
                    }
                });

        if (storageFiles.Count < 1)
        {
            context.SetOutput(null);
            return;
        }

        await using var stream = await storageFiles[0].OpenReadAsync();
        using var streamReader = new StreamReader(stream);
        var fileContent = await streamReader.ReadToEndAsync();

        context.SetOutput(fileContent);
    }


    private async Task ExportMazeInteractionHandler(InteractionContext<string?, string?> context)
    {
        var topLevel = TopLevel.GetTopLevel(this);

        var file = await topLevel.StorageProvider
            .SaveFilePickerAsync(
                new FilePickerSaveOptions
                {
                    Title = "Save Maze File",
                    FileTypeChoices = new[]
                    {
                        new FilePickerFileType("txt")
                        {
                            Patterns = new[]
                            {
                                "*.txt"
                            }
                        }
                    }
                });

        if (file is null)
        {
            context.SetOutput(null);
            return;
        }

        await using var stream = await file.OpenWriteAsync();
        await using var streamWriter = new StreamWriter(stream);
        await streamWriter.WriteAsync(context.Input);

        context.SetOutput(null);
    }
}