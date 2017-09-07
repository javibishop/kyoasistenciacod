namespace webkyo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class xcs : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Asistencia", "Alumno_Id", "dbo.Alumno");
            DropForeignKey("dbo.Examen", "Alumno_Id", "dbo.Alumno");
            DropIndex("dbo.Asistencia", new[] { "Alumno_Id" });
            DropIndex("dbo.Examen", new[] { "Alumno_Id" });
            RenameColumn(table: "dbo.Asistencia", name: "Alumno_Id", newName: "AlumnoId");
            RenameColumn(table: "dbo.Examen", name: "Alumno_Id", newName: "AlumnoId");
            AlterColumn("dbo.Asistencia", "AlumnoId", c => c.Int(nullable: false));
            AlterColumn("dbo.Examen", "AlumnoId", c => c.Int(nullable: false));
            CreateIndex("dbo.Asistencia", "AlumnoId");
            CreateIndex("dbo.Examen", "AlumnoId");
            AddForeignKey("dbo.Asistencia", "AlumnoId", "dbo.Alumno", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Examen", "AlumnoId", "dbo.Alumno", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Examen", "AlumnoId", "dbo.Alumno");
            DropForeignKey("dbo.Asistencia", "AlumnoId", "dbo.Alumno");
            DropIndex("dbo.Examen", new[] { "AlumnoId" });
            DropIndex("dbo.Asistencia", new[] { "AlumnoId" });
            AlterColumn("dbo.Examen", "AlumnoId", c => c.Int());
            AlterColumn("dbo.Asistencia", "AlumnoId", c => c.Int());
            RenameColumn(table: "dbo.Examen", name: "AlumnoId", newName: "Alumno_Id");
            RenameColumn(table: "dbo.Asistencia", name: "AlumnoId", newName: "Alumno_Id");
            CreateIndex("dbo.Examen", "Alumno_Id");
            CreateIndex("dbo.Asistencia", "Alumno_Id");
            AddForeignKey("dbo.Examen", "Alumno_Id", "dbo.Alumno", "Id");
            AddForeignKey("dbo.Asistencia", "Alumno_Id", "dbo.Alumno", "Id");
        }
    }
}
