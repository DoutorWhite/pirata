using Content.Client.Stylesheets;
using Content.Client.UserInterface.Controls;
using Content.Shared.Chemistry;
using Content.Shared.Chemistry.Reagent;
using Robust.Client.AutoGenerated;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.XAML;
using Robust.Client.Utility;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;
using System.Linq;
using System.Numerics;
using Content.Shared.FixedPoint;
using static Robust.Client.UserInterface.Controls.BoxContainer;
using Content.Shared.Chemistry.Prototypes;
using Robust.Client.GameObjects;
using System.Text;
using Content.Shared.Chemistry.Reaction;
using Content.Client.Lathe.UI;
using System.Diagnostics;
using Content.Shared.Research.Prototypes;

namespace Content.Client.Chemistry.UI
{

    [GenerateTypedNameReferences]
    public sealed partial class MedipenRefillerWindow : FancyWindow
    {
        [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
        public List<ProtoId<MedipenRecipePrototype>> Recipes = new();


        public MedipenRefillerWindow()
        {
            RobustXamlLoader.Load(this);
            IoCManager.InjectDependencies(this);
        }

        public void UpdateRecipes()
        {
            var recipesToShow = new List<MedipenRecipePrototype>();

            foreach (var recipe in Recipes)
            {
                if (!_prototypeManager.TryIndex(recipe, out var proto))
                    continue;

                if (SearchBar.Text.Trim().Length != 0)
                {
                    if (proto.Name.ToLowerInvariant().Contains(SearchBar.Text.Trim().ToLowerInvariant()))
                        recipesToShow.Add(proto);
                }
                else
                {
                    recipesToShow.Add(proto);
                }
            }

            MedipenList.Children.Clear();

            foreach (var prototype in recipesToShow)
            {
                StringBuilder sb = new();
                sb.AppendLine(prototype.Name);
                sb.AppendLine(prototype.Description);
                foreach (var reagent in prototype.RequiredReagents)
                {
                    sb.AppendLine(string.Format("{0}: {1}", reagent.Reagent, reagent.Quantity));
                }
                Control control = new RecipeControl(prototype, sb.ToString(), true);
                MedipenList.AddChild(control);
            }
        }
    }
}