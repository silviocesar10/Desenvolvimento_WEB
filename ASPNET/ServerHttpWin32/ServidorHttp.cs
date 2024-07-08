using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;

class ServidorHttp{
    private TcpListener Controlador {get;  set;}
    private int Porta {get; set;}
    private int QtdeRequests {get; set;}

    private string HtmlExemplo {get; set;}

    private SortedList<string, string> TipoMime {get; set;}
    private SortedList<string, string> DiretoriosHosts {get; set;}

    public ServidorHttp (int porta = 8080 /*,path */){
        this.Porta =porta;

        this.CriarHtmlExemplo();

        this.PupolarTiposMime();

        this.PopularDiretoriosHosts();
        try
        {
            this.Controlador = new TcpListener(IPAddress.Parse("127.0.0.1"), this.Porta);
            this.Controlador.Start();
            Console.WriteLine($"Servidot HTTP esta rodando na porta {this.Porta}.");
            Console.WriteLine($"Para acessar, digite no navegador: http://localhost:{this.Porta}");
            Task taskServidorRequest = Task.Run(() => AguardarRequests());
            taskServidorRequest.GetAwaiter().GetResult();
        }catch(Exception e)
        {
            Console.WriteLine($"Erro ao iniciar o servidor na porta{this.Porta}\n{e.Message}");
        }
    }

    private async Task AguardarRequests()
    {
        while(true)
        {
            Socket conexao = await this.Controlador.AcceptSocketAsync();
            this.QtdeRequests++;
            Task task = Task.Run(() => ProcessarRequest(conexao, this.QtdeRequests));
        }
    }

    private void ProcessarRequest(Socket conexao, int numeroRequest)
    {
        Console.WriteLine($"Processando request #{numeroRequest}....\n");
        if(conexao.Connected)
        {
            byte [] bytesRequisicao = new byte[1024];
            conexao.Receive(bytesRequisicao, bytesRequisicao.Length, 0);
            string textoRequisicao = Encoding.UTF8.GetString(bytesRequisicao)
            .Replace((char)0, ' ').Trim();
            if(textoRequisicao.Length > 0)
            {
                    Console.WriteLine($"\n{textoRequisicao}\n");

                    string[] linhas = textoRequisicao.Split("\r\n");
                    int iPrimeiroEspaco = linhas[0].IndexOf(' ');
                    int iSegundoEspaco = linhas[0].LastIndexOf(' ');
                    string metodoHttp = linhas[0].Substring(0, iPrimeiroEspaco);
                    string recursoBuscado = linhas[0].Substring(iPrimeiroEspaco + 1, iSegundoEspaco - iPrimeiroEspaco - 1);
                    if(recursoBuscado == "/") recursoBuscado = "/index.html";
                    string textoParametro = recursoBuscado.Contains("?") ? recursoBuscado.Split("?")[1] : "";
                    SortedList<string, string> parametros = ProcessarParametros(textoParametro);
                    recursoBuscado = recursoBuscado.Split("?")[0];
                    string versaoHttp = linhas[0].Substring(iSegundoEspaco + 1);
                    iPrimeiroEspaco = linhas[1].IndexOf(' ');
                    string nomeHost = linhas[1].Substring(iPrimeiroEspaco +1);

                     byte[] bytesConteudo = null;
                     byte[]  bytesCabecalho = null;
                     FileInfo fiArquivo = new FileInfo(ObterCaminhoFisicoArquivo(nomeHost, recursoBuscado)); 
                    if(fiArquivo.Exists)
                    {
                        if(TipoMime.ContainsKey(fiArquivo.Extension.ToLower()))
                        {
                            //bytesConteudo = File.ReadAllBytes(fiArquivo.FullName);
                            if(fiArquivo.Extension.ToLower() == ".dhtml")
                            {
                                bytesConteudo = GerarHtmlDinamico(fiArquivo.FullName, parametros);
                            }else
                            {
                                bytesConteudo = File.ReadAllBytes(fiArquivo.FullName);
                            }
                            
                            string tipoMime = TipoMime[fiArquivo.Extension.ToLower()];
                            bytesCabecalho= GerarCabecalho(versaoHttp, tipoMime, "200" ,bytesConteudo.Length);
                        }else
                        {
                            bytesConteudo = Encoding.UTF8.GetBytes("<h1>Erro 415 - Tipo de arquivo não suportado</h1>");
                            bytesCabecalho = GerarCabecalho(versaoHttp , "text/html;chearset=utf-8", "415", bytesConteudo.Length);
                        }
                        
                    }
                    else
                    {
                            bytesConteudo = Encoding.UTF8.GetBytes("<h1>Erro 404 - Arquivo Não Encontrado</h1>");
                            bytesCabecalho =GerarCabecalho(versaoHttp, "text/html;charset=utf-8", "404" ,bytesConteudo.Length);
                    }
                    int bytesEnviados = conexao.Send(bytesCabecalho, bytesCabecalho.Length, 0);
                    bytesEnviados += conexao.Send(bytesConteudo, bytesConteudo.Length, 0);
                    conexao.Close();
                    
                    Console.WriteLine($"{bytesEnviados} bytes enviados em resposta a requisicao {numeroRequest}.");
            }
        }
        Console.WriteLine($"Request {numeroRequest} finalizado.");
    }
    
    public byte[] GerarCabecalho(string versaoHttp, string tipoMime, string codigoHttp, int qtdBytes)
    {
        StringBuilder texto = new StringBuilder();
        texto.Append($"{versaoHttp} {codigoHttp}{Environment.NewLine}");
        texto.Append($"Server: Servidor Http Simples 1.0{Environment.NewLine}");
        texto.Append($"Content-Type: {tipoMime}{Environment.NewLine}");
        texto.Append($"Content-Length{qtdBytes}{Environment.NewLine}{Environment.NewLine}");
        return Encoding.UTF8.GetBytes(texto.ToString());
    }

    private void CriarHtmlExemplo()
    {
        StringBuilder html = new StringBuilder();
        html.Append("<!DOCTYPE html> <html lang=\"pt-br\"> <head><meta charset=\"UTF8\">");
        html.Append("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
        html.Append("<title> Pagina Estatica</title></head><body>");
        html.Append("<h1> Pagina Estatica</h1> </body></html>");
        this.HtmlExemplo = html.ToString();
    }
   /* private byte[] LerArquivo(string recurso)
    {
        string diretorio= "//home//silvio//Documentos//DW//ServerHttp//www//";
        string caminhoCompleto = diretorio + recurso.Replace("/", "//");
        if(File.Exists(caminhoCompleto))
        {
            return File.ReadAllBytes(caminhoCompleto);
        }
        else return new byte[0];
    }*/
    
    private void PupolarTiposMime()
    {
        this.TipoMime = new SortedList<string, string>();
        this.TipoMime.Add(".html", "text/html;charset=utf-8");
        this.TipoMime.Add(".dhtml", "text/html;charset=utf-8");
        this.TipoMime.Add(".htm", "text/html;charset=utf-8");
        this.TipoMime.Add(".css", "text/css");
        this.TipoMime.Add(".js", "text/javascript");
        this.TipoMime.Add(".png", "image/png");
        this.TipoMime.Add(".gif", "image/gif");
        this.TipoMime.Add(".svg", "image/svg+xml");
        this.TipoMime.Add(".ico", "image/ico");
        this.TipoMime.Add(".woff", "font/woff");
        this.TipoMime.Add(".woff2", "font/woff2");
    }
    private void PopularDiretoriosHosts() 
    {
        this.DiretoriosHosts = new SortedList<string, string>();
        this.DiretoriosHosts.Add("localhost", "//home//silvio//Documentos//DW//ServerHttp//www//localhost//");
        this.DiretoriosHosts.Add("silvio.com", "//home//silvio//Documentos//DW//ServerHttp//www//silviocesarsilvaoliveira.com//");
        this.DiretoriosHosts.Add("quitandaonline.com.br","//home//silvio//Documentos//DW//ServerHttp//www//QuitandaOnlineBS5//");
    }

    public string ObterCaminhoFisicoArquivo(string host,string arquivo)
    {
     string diretorio = this.DiretoriosHosts[host.Split(":")[0]];
     string caminhoArquivo = diretorio + arquivo.Replace("/", "//");
     return caminhoArquivo;    
    }

    public byte[] GerarHtmlDinamico(string caminhoArquivo,SortedList<string, string> parametros)
    {
        string coringa = "{{Dinamico}}";
        string htmlModelo = File.ReadAllText(caminhoArquivo);
        StringBuilder htmlGerado = new StringBuilder();
        /*htmlGerado.Append("<ul>");
        foreach(var i in this.TipoMime.Keys)
        {
            htmlGerado.Append($"<li>Arquivos com extensao {i}</li>");
        }  
        htmlGerado.Append("</ul>");*/
        if(parametros.Count > 0)
        {
            htmlGerado.Append("<ul>");
            foreach(var p in parametros)
            {
                htmlGerado.Append($"<li>{p.Key}={p.Value}</li>");
            }  
            htmlGerado.Append("</ul>");
        }
        else
        {
            htmlGerado.Append("<p>Nenhum parametro foi passado!!</p>");
        }
        string textoGerado = htmlModelo.Replace(coringa, htmlGerado.ToString());
        return Encoding.UTF8.GetBytes(textoGerado, 0, textoGerado.Length);
    }
    private SortedList<string, string> ProcessarParametros(string textoParametro)
    {
        SortedList<string, string> parametros = new SortedList<string, string>();
        if(!string.IsNullOrEmpty(textoParametro.Trim()))
        {
            string[] paresChaveValor = textoParametro.Split("&");
            foreach(var par in paresChaveValor)
            {
                parametros.Add(par.Split("=")[0].ToLower(),par.Split("=")[1]);
            }
        }
        return parametros;
    }
}
