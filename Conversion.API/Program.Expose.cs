// Rendre la classe Program publique pour que le projet de tests puisse
// utiliser WebApplicationFactory<Program> pour les tests d'intégration.
// Avec les "top-level statements", le compilateur génère une classe Program
// en interne ; ce fichier partial la rend publique.
namespace Conversion.API;

public partial class Program
{
}
