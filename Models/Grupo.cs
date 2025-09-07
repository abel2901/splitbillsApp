﻿namespace SplitBillsApi.Models
{
    public class Grupo
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;

        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
        public ICollection<Gasto> Gastos { get; set; } = new List<Gasto>();
    }
}
