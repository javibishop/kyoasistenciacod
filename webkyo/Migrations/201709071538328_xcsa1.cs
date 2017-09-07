namespace webkyo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class xcsa1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Alumno", "CinturonId", "dbo.Cinturon");
            DropForeignKey("dbo.Alumno", "DojoId", "dbo.Dojo");
            DropForeignKey("dbo.Asistencia", "AlumnoId", "dbo.Alumno");
            DropForeignKey("dbo.Examen", "AlumnoId", "dbo.Alumno");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            AddForeignKey("dbo.Alumno", "CinturonId", "dbo.Cinturon", "Id");
            AddForeignKey("dbo.Alumno", "DojoId", "dbo.Dojo", "Id");
            AddForeignKey("dbo.Asistencia", "AlumnoId", "dbo.Alumno", "Id");
            AddForeignKey("dbo.Examen", "AlumnoId", "dbo.Alumno", "Id");
            AddForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles", "Id");
            AddForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Examen", "AlumnoId", "dbo.Alumno");
            DropForeignKey("dbo.Asistencia", "AlumnoId", "dbo.Alumno");
            DropForeignKey("dbo.Alumno", "DojoId", "dbo.Dojo");
            DropForeignKey("dbo.Alumno", "CinturonId", "dbo.Cinturon");
            AddForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Examen", "AlumnoId", "dbo.Alumno", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Asistencia", "AlumnoId", "dbo.Alumno", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Alumno", "DojoId", "dbo.Dojo", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Alumno", "CinturonId", "dbo.Cinturon", "Id", cascadeDelete: true);
        }
    }
}
