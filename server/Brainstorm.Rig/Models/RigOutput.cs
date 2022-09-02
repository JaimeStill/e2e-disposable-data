namespace Brainstorm.Rig.Models;
public struct RigOutput
{
    public RigState State { get; set; }
    public RigMessage Output { get; set; }
    public bool Exiting { get; set; }    
}