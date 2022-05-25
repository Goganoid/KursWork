using Tenders.Controllers;
using Tenders.Models;

namespace Tenders.Data;

public static class DbInitializer
{
    public static Tender RandomTender()
    {
        var titles = new[]
        {
            "Закупівля пломб пластмасових для потреб ПрАТ «Дубномолоко»",
            "Технічний огляд та експертне обстеження (технічне діагностування) газопроводів",
            "Поставка комплектуючих до металопластикових конструкцій.",
            "Роботи з технічного огляду та експертного обстеження (технічне діагностування)",
            "Поставка запчастин до автобусного, вантажного та технологічного транспорту",
            "Фильтры для компрессоров 2022",
            "Заготовка профільна для локомотивних осей",
            "Дизельне паливо, Бензин (92 чи 95)",
            "Этикетка самоклейка рулонная полуглянец 58х60мм ч/б в ассортименте",
            "Світильники 2 квартал 2022 року",
            "Газоэлектросварочное оборудование_2 квартал 2022г",
            "Вентиляторное оборудование_2 квартал 2022г",
            "Изделия из фторопласта",
            "Поставка цементу для ПАО \"Полтаваобленерго\"",
            "Кондиционеры и запасные части к холодильному оборудованию 2 квартал 2022",
            "Поставка керамічної цегли для АТ \"Марганецький ГЗК\", м. Марганець, Дніпропетровська обл.",
            "Поставка лакофарбових матеріалів у 2-му кв. 2022р.",
            "Поставка запчастин до автобусної техніки в 2 кв. 2022р для АТ \"НЗФ\"",
            "Ставка з ПДФ м. Дніпро вул. Юдіна 11- м. Полтава(36040), м.Чернівці(58020)",
            "Закупівля послуги контролю за шкідниками",
            "Клапани"
        };


        return new Tender
        {
            Title = titles[Random.Shared.Next(titles.Length)],
            EndDate = DateTime.Now.AddDays(Random.Shared.Next(40)),
            PubDate = DateTime.Now.Subtract(TimeSpan.FromDays(Random.Shared.Next(40))),
            Cost =(uint) Random.Shared.Next(1,20) * 10000,
            IsActive = true
        };
    }

    public static void Initialize(ApplicationContext context)
    {
        // Look for any students.
        if (context.Users.Any()) return; // DB has been seeded
        var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true, true);
        var config = builder.Build();

        var tender1 = RandomTender();
        var tender2 = RandomTender();
        var tender3 = RandomTender();
        var tender4 = RandomTender();
        var company1 = new Company("Зелений парк", "Хмельницька обл, м. Ізяслав");
        company1.Tenders = new List<Tender> {tender1, tender2};
        var company12 = new Company("ЗАКУПІВЛІ КОМ", "Хмельницька обл");
        var company2 = new Company("Марганецький ГЗК", "Київська обл, м. Деричі");
        company2.Tenders = new List<Tender> {tender3, tender4};

        var company3 = new Company("ВусаЛапиХвіст", "Київська обл, м. Іваново");
        var password = AuthenticateController.CalculateSha256("Passw0rd123", config.GetValue<string>("Auth:salt"));

        var users = new User[]
        {
            new("John","Biden", "testmail1@gmail.com",password, new List<Company> {company1,company12}),
            new("Alex","Bob","testmail2@gmail.com",password,new List<Company> {company2}),
            new("Marcy","Killy","testmail3@gmail.com", password, new List<Company> {company3}),
            new("Adolf","Don","testmail4@gmail.com",password)
        };
        context.Users.AddRange(users);
        context.SaveChanges();
        company1 = context.Companies.Find(1)!;
        company2 = context.Companies.Find(2)!;
        company2 = context.Companies.Find(3)!;
        context.Propositions.Add(new Proposition {Company = company2, Cost = 6000, Tender = tender1});
        context.Propositions.Add(new Proposition {Company = company3, Cost = 7000, Tender = tender1});
        context.SaveChanges();
 
    }
}