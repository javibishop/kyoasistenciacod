namespace webkyo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vv : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Alumno", "Cinturon_Id", "dbo.Cinturon");
            DropForeignKey("dbo.Alumno", "Dojo_Id", "dbo.Dojo");
            DropIndex("dbo.Alumno", new[] { "Cinturon_Id" });
            DropIndex("dbo.Alumno", new[] { "Dojo_Id" });
            RenameColumn(table: "dbo.Alumno", name: "Cinturon_Id", newName: "CinturonId");
            RenameColumn(table: "dbo.Alumno", name: "Dojo_Id", newName: "DojoId");
            AlterColumn("dbo.Alumno", "CinturonId", c => c.Int(nullable: false));
            AlterColumn("dbo.Alumno", "DojoId", c => c.Int(nullable: false));
            CreateIndex("dbo.Alumno", "DojoId");
            CreateIndex("dbo.Alumno", "CinturonId");
            AddForeignKey("dbo.Alumno", "CinturonId", "dbo.Cinturon", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Alumno", "DojoId", "dbo.Dojo", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Alumno", "DojoId", "dbo.Dojo");
            DropForeignKey("dbo.Alumno", "CinturonId", "dbo.Cinturon");
            DropIndex("dbo.Alumno", new[] { "CinturonId" });
            DropIndex("dbo.Alumno", new[] { "DojoId" });
            AlterColumn("dbo.Alumno", "DojoId", c => c.Int());
            AlterColumn("dbo.Alumno", "CinturonId", c => c.Int());
            RenameColumn(table: "dbo.Alumno", name: "DojoId", newName: "Dojo_Id");
            RenameColumn(table: "dbo.Alumno", name: "CinturonId", newName: "Cinturon_Id");
            CreateIndex("dbo.Alumno", "Dojo_Id");
            CreateIndex("dbo.Alumno", "Cinturon_Id");
            AddForeignKey("dbo.Alumno", "Dojo_Id", "dbo.Dojo", "Id");
            AddForeignKey("dbo.Alumno", "Cinturon_Id", "dbo.Cinturon", "Id");
        }
    }
}
