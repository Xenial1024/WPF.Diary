namespace Diary.Migrations
{
    using System.CodeDom.Compiler;
    using System.Data.Entity.Migrations.Infrastructure;
    using System.Resources;

    [GeneratedCode("EntityFramework.Migrations", "6.2.0-61023")]
    public sealed partial class SetFirstNameMaxLengthAndRequiredInStudentsTable : IMigrationMetadata
    {
        private readonly ResourceManager resourceManager = new ResourceManager(typeof(SetFirstNameMaxLengthAndRequiredInStudentsTable));

        string IMigrationMetadata.Id => "202411031028278_SetFirstNameMaxLengthAndRequiredInStudentsTable";
        string IMigrationMetadata.Source => null;
        string IMigrationMetadata.Target => resourceManager.GetString("Target");
    }
}
