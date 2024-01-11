using Telerik.Windows.Controls;

namespace KassaLib
{
    public class CustomLocalizationManager : LocalizationManager
    {
        public override string GetStringOverride(string key)
        {
            switch (key)
            {
                case "Clear":
                    return "Очистить";
                case "Close":
                    return "Закрыть";
                case "TimeSpanPicker_Minutes":
                    return "Минуты";
                case "Month":
                    return "Месяц";
                case "EnterDate":
                    return "Введите дату";
                case "EnterTime":
                    return "Введите время";
                case "GridViewSearchPanelTopText":
                    return "Контекстный поиск";
                case "GridViewGroupPanelText":
                    return "Перетащите заголовок колонки сюда, чтобы выполнить группировку";
                case "GridViewClearFilter":
                    return "Очистить фильтр";
                case "GridViewFilterAnd":
                    return "И";
                case "GridViewFilterContains":
                    return "Содержит";
                case "GridViewFilterDoesNotContain":
                    return "Не содержит";
                case "GridViewFilterEndsWith":
                    return "Заканчивается на";
                case "GridViewFilterIsContainedIn":
                    return "Содержится в";
                case "GridViewFilterIsEqualTo":
                    return "Равно";
                case "GridViewFilterIsGreaterThan":
                    return "Больше чем";
                case "GridViewFilterIsGreaterThanOrEqualTo":
                    return "Больше или равно чем";
                case "GridViewFilterIsNotContainedIn":
                    return "Не содержится в";
                case "GridViewFilterIsLessThan":
                    return "Меньше чем";
                case "GridViewFilterIsLessThanOrEqualTo":
                    return "Меньше или равно чем";
                case "GridViewFilterIsNotEqualTo":
                    return "Не равно";
                case "GridViewFilterOr":
                    return "Или";
                case "GridViewFilterSelectAll":
                    return "Выбрать все";
                case "GridViewFilterStartsWith":
                    return "Начнается с";
                case "GridViewFilterIsNull":
                    return "Нуль";
                case "GridViewFilterIsNotNull":
                    return "Не нуль";
                case "GridViewFilterIsEmpty":
                    return "Пусто";
                case "GridViewFilterIsNotEmpty":
                    return "Не пусто";
                case "GridViewFilterShowRowsWithValueThat":
                    return "Показать строки удовлетворяющие условию";
                case "Wizard_Cancel":
                    return "Отмена";
                case "Wizard_Next":
                    return "Далее >";
                case "Wizard_Previous":
                    return "< Назад";
                case "Wizard_Finish":
                    return "Завершить";
            }

            return base.GetStringOverride(key);
        }
    }
}
