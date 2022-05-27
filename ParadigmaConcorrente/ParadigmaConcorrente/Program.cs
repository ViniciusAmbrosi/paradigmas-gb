public class Program
{
    public static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1);

    public static void Main(String[] args)
    {
        List<int> lista = new List<int>();

        Console.WriteLine("\nExecutando em thread sem sincronia");
        new Thread(() => AdicionaNaLista(lista, 10, 1000, false)).Start();
        new Thread(() => ExibeLista(lista, 0, false)).Start();

        Thread.Sleep(2000);
        lista = new List<int>();

        Console.WriteLine("\nExecutando em thread com sincronia");
        new Thread(() => AdicionaNaLista(lista, 20, 1000, true)).Start();
        new Thread(() => ExibeLista(lista, 0, true)).Start();

        Thread.Sleep(2000);
        lista = new List<int>();

        Console.WriteLine("\nExecutando com tasks sem espera");
        TaskAdicionaNaLista(lista, 30, 1000);
        TaskExibeLista(lista, 0);

        Thread.Sleep(2000);
        lista = new List<int>();

        //Console.WriteLine("Executando com tasks em espera");
        //await TaskAdicionaNaLista(lista, 30, 1000);
        //await TaskExibeLista(lista, 0);
    }

    public static void AdicionaNaLista(List<int> lista, int valorParSerAdicionado, int tempoDeProcessamento = 0, bool usarSemaforo = false)
    {
        if (usarSemaforo)
        {
            semaphoreSlim.Wait();
        }

        try
        {
            Thread.Sleep(tempoDeProcessamento);

            Console.WriteLine("Inserindo " + valorParSerAdicionado);
            lista.Add(valorParSerAdicionado);
        }
        finally {
            if (usarSemaforo)
            {
                semaphoreSlim.Release();
            }
        }
    }

    public static void ExibeLista(List<int> lista, int tempoDeProcessamento = 0, bool usarSemaforo = false)
    {
        if (usarSemaforo) {
            semaphoreSlim.Wait();
        }

        try
        {
            Thread.Sleep(tempoDeProcessamento);

            if (lista.Count == 0)
            {
                Console.WriteLine("Lista Vazia");
            }
            else
            {
                Console.WriteLine("Valores da lista");
                foreach (int valor in lista)
                {
                    Console.Write(valor + " ");
                }

                Console.WriteLine();
            }
        }
        finally
        {
            if (usarSemaforo)
            {
                semaphoreSlim.Release();
            }
        }
    }

    public static async Task TaskAdicionaNaLista(List<int> lista, int valorParSerAdicionado, int tempoDeProcessamento = 0)
    {
        await Task.Delay(tempoDeProcessamento);

        lista.Add(valorParSerAdicionado);
    }

    public static async Task TaskExibeLista(List<int> lista, int tempoDeProcessamento = 0)
    {
        await Task.Delay(tempoDeProcessamento);

        if (lista.Count == 0)
        {
            Console.WriteLine("Lista Vazia");
        }
        else
        {
            Console.WriteLine("Valores da lista");
            foreach (int valor in lista)
            {
                Console.Write(valor + " ");
            }
        }
    }
}