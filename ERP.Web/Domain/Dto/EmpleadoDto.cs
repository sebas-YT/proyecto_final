namespace ERP.Web.Domain.Dto
{
    public class EmpleadoDto
    {
        public int Id { get; set; }//No requerir (Generado por la DB)
        public int PersonaId { get; set; }//No requerir (Generado por la DB)
        public decimal Sueldo { get; set; }//Si
        public PersonaDto DatosPersonales { get; set; } = new PersonaDto();
    }
}
