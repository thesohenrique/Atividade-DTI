using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtividadeDTI
{
    class Program
    {
        static void Main(string[] args)
        {
                string[] lines = System.IO.File.ReadAllLines(@"C:\Users\mathe\OneDrive\Área de Trabalho\DTI\AtividadeDTI\dados.txt"); //Guarda cada linha do arquivo .txt em uma posição do array.
                string[] dados;
                string[] separador = { "-%p" }; //Variável para guardar o conjunto de caracteres especiais que irá separar cada dado de entrada.
                string uuid, uuidDestino, nome, data;
                double saldoConta, valorTransf;
                List<Usuario> usuarios = new List<Usuario>(); //Lista de Usuarios que farão transações que será passada por parametro para o banco

                foreach (string line in lines) //Para cada linha do arquivo .txt, faz-se:
                {
                    dados = line.Split(separador, System.StringSplitOptions.RemoveEmptyEntries); //Guarda cada dado, separados pela variavel separador, em uma posição do array.
                    uuid = dados[0]; //Cada variável que representa um dado de entrada receberá seu respectivo valor:
                    saldoConta = double.Parse(dados[1].Replace(".",","));
                    nome = dados[2];
                    data = dados[3];
                    valorTransf = double.Parse(dados[4].Replace(".", ","));
                    uuidDestino = dados[5];
                    usuarios.Add(new Usuario(uuid, saldoConta, nome, data, valorTransf, uuidDestino));//Cria um objeto Usuario e passa para seu construtor todos os dados referente a ele, inserindo-o na lista.
                }

                Banco banco = new Banco(usuarios);
                banco.gerenciarTransacoes();//Instancia do método que executará as transações.
            
          
            Console.ReadKey();
        }
    }
    class Usuario //Classe que representa cada usuario com seus respectivos dados, como tambem os métodos para retorná-los quando necessario.
    {
        public string uuid, uuidDestino, nome, data, dataCorreta;
        public double saldoConta, valorTransf;

        public Usuario(string uuid, double saldoConta, string nome, string data, double valorTransf, string uuidDestino)
        {
            this.uuid = uuid;
            this.uuidDestino = uuidDestino;
            this.nome = nome;
            this.data = data;
            this.saldoConta = saldoConta;
            this.valorTransf = valorTransf;
        }

        public string getNome()
        {
            return nome;
        }

        public double getSaldoConta()
        {
           return saldoConta;
        }

        public string getData()
        {
            dataCorreta = DateTime.Parse(data).ToString("dd/MM/yyyy"); //Converte a data para o formato pedido na saída.
            return dataCorreta;
        }

        public double getValorTransf()
        {
            return valorTransf;
        }

        public string getUuidDestino()
        {
            return uuidDestino;
        }

        public string getUuid()
        {
            return uuid;
        }

        public string getNovoSaldoConta() //Retorna o novo saldo em conta do usuario, no formato monetario, após a trasação.
        {
            if (saldoConta >= valorTransf)
            {
                saldoConta -= valorTransf;
                return saldoConta.ToString("C");
            }
            else //Caso o usuario n possua saldo suficiente, o valor nao será descontado e a transação nao será efetuada.
            {
                return saldoConta.ToString("C");
            }
        }
    }

    class Banco //Classe que representa o banco e realiza a transação.
    {
        List<Usuario> usuarios;

        public Banco(List<Usuario> usuarios)
        {
            this.usuarios = usuarios;
        }

        public void gerenciarTransacoes()
        {
            foreach (Usuario usuario in usuarios) //Realiza a transação de cada usuario presente na lista.
            {
                foreach (Usuario u in usuarios) //Laço para poder comparar o UUID de destino de cada usuario da lista com o UUID dos demais usuarios, pegando assim o nome do usuario de destino da transação. 
                {
                    if (usuario.getUuidDestino() == u.getUuid())
                    {
                        if (usuario.getSaldoConta() >= usuario.getValorTransf())
                        {
                            Console.WriteLine(usuario.getNome() + " transferiu " + usuario.getValorTransf().ToString("C") + " para " + u.getNome() + " no dia " + usuario.getData());
                        }
                        else
                        {
                            Console.WriteLine(usuario.getNome() + " não pode transferir " + usuario.getValorTransf().ToString("C") + " para " + u.getNome() + " no dia " + usuario.getData() + " por possuir saldo insuficiente (" + usuario.getSaldoConta().ToString("C") + ")");
                        }
                    }
                }
            }
            Console.WriteLine("Saldos:");
            foreach (Usuario usuario in usuarios)
            {
                Console.WriteLine(usuario.getNome() +" - "+usuario.getNovoSaldoConta());
            }
        }
    }
}
