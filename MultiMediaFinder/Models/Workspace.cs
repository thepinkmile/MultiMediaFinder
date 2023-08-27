using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MultiMediaFinder.Models;

public class Workspace
{
    public readonly string WorkspaceId = Guid.NewGuid().ToString();

    public string WorkspacePath
    {
        get
        {
            return Path.Combine(Path.GetTempPath(), WorkspaceId);
        }
    }

    public async Task InitialiseWorkspace(CancellationToken cancellationToken)
    {
        Directory.CreateDirectory(WorkspacePath);

        await Task.Delay(1_000, cancellationToken);
    }

    public async Task CloseWorkspace(CancellationToken cancellationToken)
    {
        await Task.Delay(1_000, cancellationToken);

        Directory.Delete(WorkspacePath, true);
    }
}
