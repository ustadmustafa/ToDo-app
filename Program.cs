namespace ToDo_app;

class Program
{
    static void Main(string[] args)
    {
        // Varsayılan board oluştur
        Board board = new Board();

        // Takım üyelerini tanımla
        Dictionary<int, string> teamMembers = new Dictionary<int, string>
        {
            { 1, "TakimUye1" },
            { 2, "TakimUye2" },
            // Diğer takım üyelerini ekleyin
        };

        while (true)
        {
            Console.WriteLine("Lütfen yapmak istediğiniz işlemi seçiniz:");
            Console.WriteLine("(1) Board Listelemek");
            Console.WriteLine("(2) Board'a Kart Eklemek");
            Console.WriteLine("(3) Board'dan Kart Silmek");
            Console.WriteLine("(4) Kart Taşımak");
            Console.WriteLine("(5) Çıkış");

            int islem;
            if (!int.TryParse(Console.ReadLine(), out islem))
            {
                Console.WriteLine("Hatalı giriş yaptınız. Lütfen tekrar deneyin.");
                continue;
            }

            switch (islem)
            {
                case 1:
                    // Board listeleme işlemi
                    board.ListBoard();
                    break;
                case 2:
                    // Board'a kart ekleme işlemi
                    board.AddCard(teamMembers);
                    break;
                case 3:
                    // Kart silme işlemi
                    board.RemoveCard();
                    break;
                case 4:
                    // Kart taşıma işlemi
                    board.MoveCard();
                    break;
                case 5:
                    // Uygulamadan çıkış yapma
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Hatalı bir seçim yaptınız. Lütfen tekrar deneyin.");
                    break;
            }
        }
    }
}

// Kartın büyüklüklerini tanımlayan enum
enum CardSize
{
    XS,
    S,
    M,
    L,
    XL
}

// Kart sınıfı
class Card
{
    public string Title { get; set; }
    public string Content { get; set; }
    public int AssignedTo { get; set; }
    public CardSize Size { get; set; }

    public Card(string title, string content, int assignedTo, CardSize size)
    {
        Title = title;
        Content = content;
        AssignedTo = assignedTo;
        Size = size;
    }
}

// Line sınıfı
class Line
{
    public List<Card> Cards { get; set; } = new List<Card>();
}

// Board sınıfı
class Board
{
    private List<Line> Lines { get; set; }

    public Board()
    {
        // Varsayılan board oluştur
        Lines = new List<Line>
        {
            new Line(),
            new Line(),
            new Line()
        };
    }

    public void ListBoard()
    {
        for (int i = 0; i < Lines.Count; i++)
        {
            string lineName = ((BoardLine)i).ToString();
            Console.WriteLine($"{lineName} Line");
            Console.WriteLine("************************");

            foreach (var card in Lines[i].Cards)
            {
                Console.WriteLine($"Başlık: {card.Title}");
                Console.WriteLine($"İçerik: {card.Content}");
                Console.WriteLine($"Atanan Kişi: {card.AssignedTo}");
                Console.WriteLine($"Büyüklük: {card.Size}");
                Console.WriteLine("-");
            }

            if (Lines[i].Cards.Count == 0)
            {
                Console.WriteLine("~ BOŞ ~");
            }
        }
    }

    public void AddCard(Dictionary<int, string> teamMembers)
    {
        // Kart eklemek için gerekli bilgileri alın
        Console.Write("Başlık Giriniz: ");
        string title = Console.ReadLine();
        Console.Write("İçerik Giriniz: ");
        string content = Console.ReadLine();

        Console.WriteLine("Büyüklük Seçiniz -> XS(1), S(2), M(3), L(4), XL(5): ");
        if (!Enum.TryParse<BoardLine>(Console.ReadLine(), out BoardLine line))
        {
            Console.WriteLine("Hatalı giriş yaptınız. Kart eklenemedi.");
            return;
        }

        Console.WriteLine("Kişi Seçiniz:");
        foreach (var member in teamMembers)
        {
            Console.WriteLine($"({member.Key}) {member.Value}");
        }
        int assignedTo;
        if (!int.TryParse(Console.ReadLine(), out assignedTo) || !teamMembers.ContainsKey(assignedTo))
        {
            Console.WriteLine("Hatalı giriş yaptınız. Kart eklenemedi.");
            return;
        }

        Card card = new Card(title, content, assignedTo, (CardSize)line);
        Lines[(int)line].Cards.Add(card);
        Console.WriteLine("Kart başarıyla eklendi.");
    }

    public void RemoveCard()
    {
        // Kart silme işlemi
        Console.Write("Silmek istediğiniz kartın başlığını yazınız: ");
        string title = Console.ReadLine();

        bool cardRemoved = false;

        foreach (var line in Lines)
        {
            Card cardToRemove = line.Cards.FirstOrDefault(card => card.Title == title);
            if (cardToRemove != null)
            {
                line.Cards.Remove(cardToRemove);
                cardRemoved = true;
            }
        }

        if (cardRemoved)
        {
            Console.WriteLine("Kart başarıyla silindi.");
        }
        else
        {
            Console.WriteLine("Aradığınız kritere uygun kart bulunamadı.");
        }
    }

    public void MoveCard()
    {
        // Kart taşıma işlemi
        Console.Write("Taşımak istediğiniz kartın başlığını yazınız: ");
        string title = Console.ReadLine();

        bool cardFound = false;

         foreach (var fromLine in Lines)
        {
            Card cardToMove = fromLine.Cards.FirstOrDefault(card => card.Title == title);
            if (cardToMove != null)
            {
                Console.WriteLine("Bulunan Kart Bilgileri:");
                Console.WriteLine($"Başlık: {cardToMove.Title}");
                Console.WriteLine($"İçerik: {cardToMove.Content}");
                Console.WriteLine($"Atanan Kişi: {cardToMove.AssignedTo}");
                Console.WriteLine($"Büyüklük: {cardToMove.Size}");
                Console.WriteLine("Lütfen taşımak istediğiniz Line'ı seçiniz: (1) TODO, (2) IN PROGRESS, (3) DONE");
                if (int.TryParse(Console.ReadLine(), out int moveToLine) && moveToLine >= 1 && moveToLine <= 3)
            {
                Lines[moveToLine - 1].Cards.Add(cardToMove);
                fromLine.Cards.Remove(cardToMove);
                cardFound = true;
            }
            else
            {
                Console.WriteLine("Hatalı bir seçim yaptınız. Kart taşıma işlemi iptal edildi.");
            }
            break;
        }
    }

    if (!cardFound)
    {
        Console.WriteLine("Aradığınız kritere uygun kart bulunamadı.");
    }
}
}

// Board line'larını temsil eden enum
enum BoardLine
{
TODO = CardSize.XS,
IN_PROGRESS = CardSize.S,
DONE = CardSize.L
}
