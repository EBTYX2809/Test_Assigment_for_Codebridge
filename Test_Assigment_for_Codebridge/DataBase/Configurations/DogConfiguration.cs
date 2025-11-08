using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Test_Assigment_for_Codebridge.Models;

namespace Test_Assigment_for_Codebridge.DataBase.Configurations;

public class DogConfiguration : IEntityTypeConfiguration<Dog>
{
    public void Configure(EntityTypeBuilder<Dog> builder)
    {
        builder.ToTable("dogs");

        builder.HasKey(d => d.Id);

        builder.Property( d => d.Id)
            .IsRequired()
            .ValueGeneratedOnAdd()
            .HasColumnName("id");

        builder.Property( d => d.Name)
            .IsRequired()
            .HasMaxLength(20)
            .HasColumnName("name");

        builder.HasIndex(d => d.Name)
            .IsUnique();

        builder.Property( d => d.Color)
            .IsRequired()
            .HasMaxLength(20)
            .HasColumnName("color");

        builder.Property( d => d.TailLength)
            .IsRequired()
            .HasColumnType("int")
            .HasDefaultValue(0)
            .HasColumnName("tail_length");

        builder.Property( d => d.Weight)
            .IsRequired()
            .HasColumnType("int")
            .HasDefaultValue(0)
            .HasColumnName("weight");
    }
}
