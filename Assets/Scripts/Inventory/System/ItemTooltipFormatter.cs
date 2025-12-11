using System.Text;

public static class ItemTooltipFormatter
{
    public static string Format(ItemData item)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"<b>{item.displayName}</b>");

        if (!string.IsNullOrWhiteSpace(item.description))
            sb.AppendLine(item.description);

        switch (item.itemType)
        {
            case ItemType.Weapon:
                sb.AppendLine();
                if (item.physicalDamageBonus != 0)
                    sb.AppendLine($"<b>Physical Damage:</b> +{item.physicalDamageBonus}");
                if (item.magicDamageBonus != 0)
                    sb.AppendLine($"<b>Magic Damage:</b> +{item.magicDamageBonus}");
                break;

            case ItemType.Consumable:
                sb.AppendLine();
                sb.AppendLine("<b>Consumable:</b> Use to gain an effect.");
                break;

            case ItemType.PuzzleItem:
                sb.AppendLine();
                sb.AppendLine("<b>Puzzle Item</b>");
                break;

            case ItemType.Armor:
                sb.AppendLine();
                sb.AppendLine("<b>Armor</b>");
                break;
        }

        return sb.ToString();
    }
}
