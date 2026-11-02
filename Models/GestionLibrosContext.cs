using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GestionLibrosApi.Models;

public partial class GestionLibrosContext : DbContext
{
    public GestionLibrosContext()
    {
    }

    public GestionLibrosContext(DbContextOptions<GestionLibrosContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Estudiante> Estudiantes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
