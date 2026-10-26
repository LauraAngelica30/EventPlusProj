// Imports dos hooks do React para controle de estado, efeitos e otimização
import { useCallback, useEffect, useState, type ChangeEvent, type FormEvent } from "react";
// Import do componente do Rodapé
import Footer from "../../components/footer/Footer";
// Import do componente do Cabeçalho
import Header from "../../components/header/Header";
// Import do arquivo de estilização da página
import "./Usuarios.css";
// Import das tipagens TypeScript para TipoUsuario e Usuario
import type { TipoUsuario, Usuario } from "../../types/api";
// Import do componente customizado de input
import Campo from "../../components/campo/Campo";
// Import do componente customizado de botão
import Botao from "../../components/botao/Botao";
// Import dos serviços de API para manipular usuários e tipos
import { tipoUsuarioService, usuarioService } from "../../Services/eventPlusservice";
// Import da função utilitária para extrair mensagens de erro da API
import { getApiErrorMessage } from "../../Services/api";
// Import do componente da lista de usuários
import ListaUsuarios from "../../components/listaUsuarios/ListaUsuarios";
// Import do modal de confirmação de exclusão
import ModalConfirmacao from "../../components/modalConfirmacao/ModalConfirmacao";

// Interface que define os campos necessários do formulário
interface FormularioUsuario { nome: string; email: string; senha: string; idTipoUsuario: string }

// Objeto com os valores padrão zerados para o formulário
const inicial: FormularioUsuario = { nome: "", email: "", idTipoUsuario: "", senha: "" }

function Usuarios() {
    // Estado para guardar o texto digitado nos campos do formulário
    const [formulario, setFormulario] = useState(inicial);
    // Estado para guardar a lista de usuários vinda da API
    const [usuarios, setUsuarios] = useState<Usuario[]>([]);
    // Estado para guardar a lista de tipos/cargos de usuários
    const [tipos, setTipos] = useState<TipoUsuario[]>([]);
    // Estado para guardar o ID do usuário que está sendo editado (null = criando novo)
    const [idEmEdicao, setIdEmEdicao] = useState<string | null>(null);
    // Estado para guardar o usuário selecionado para excluir no modal
    const [selecionado, setSelecionado] = useState<Usuario | null>(null);
    // Estado para controlar se a página está carregando os dados
    const [carregando, setCarregando] = useState(true);
    // Estado para indicar se o formulário está enviando dados
    const [enviando, setEnviando] = useState(false);
    // Estado para guardar mensagens de sucesso, aviso ou erro
    const [mensagem, setMensagem] = useState("")

    // Função assíncrona para carregar usuários e tipos da API
    const carregar = useCallback(async () => {
        setCarregando(true); // Exibe o status de carregamento
        setMensagem(""); // Limpa mensagens antigas
        try {
            // Busca usuários e tipos ao mesmo tempo na API
            const [usuariosApi, tiposApi] = await Promise.all([usuarioService.listar(), tipoUsuarioService.listar()]);
            setUsuarios(usuariosApi); // Atualiza o estado de usuários
            setTipos(tiposApi); // Atualiza o estado de tipos
            // Define o tipo inicial padrão se não houver um selecionado
            setFormulario((value) => ({ ...value, idTipoUsuario: value.idTipoUsuario || tiposApi[0].idTipoUsuario || "" }));
        } catch (error) {
            // Guarda a mensagem caso ocorra algum erro
            setMensagem(getApiErrorMessage(error));
        } finally {
            setCarregando(false); // Esconde o status de carregamento
        }
    }, []);

    // Executa a função carregar assim que o componente é montado na tela
    useEffect(() => {
        const timeout = window.setTimeout(() => void carregar(), 0);
        return () => window.clearTimeout(timeout); // Limpa o timeout se o componente desmontar
    }, [carregar]);

    // Atualiza o valor de cada campo do formulário dinamicamente conforme digita
    function atualizar(event: ChangeEvent<HTMLInputElement | HTMLSelectElement>) {
        setFormulario((value) => ({ ...value, [event.target.name]: event.target.value }));
    }

    // Reseta o formulário para o estado inicial
    function limpar() {
        setFormulario({ ...inicial, idTipoUsuario: tipos[0]?.idTipoUsuario || "" });
        setIdEmEdicao(null); // Cancela o modo de edição
        setMensagem(""); // Limpa mensagens
    }

    // Função para salvar (cadastrar ou atualizar) o usuário
    async function salvar(event: FormEvent<HTMLFormElement>) {
        event.preventDefault(); // Impede o recarregamento da página
        setEnviando(true); // Ativa o estado de envio
        setMensagem(""); // Limpa mensagens antigas
        try {
            // Prepara os dados removendo espaços extras nas pontas
            const payload = { 
                ...formulario, 
                nome: formulario.nome.trim(),
                email: formulario.email.trim(),
                idTipoUsuario: formulario.idTipoUsuario || null
            };

            // Atualiza se estiver editando ou cadastra se for novo
            if (idEmEdicao) await usuarioService.atualizar(idEmEdicao, payload);
            else await usuarioService.cadastrar(payload);
            
            limpar(); // Reseta o formulário
            await carregar(); // Recarrega a lista atualizada
        } catch (error) {
            setMensagem(getApiErrorMessage(error)); // Exibe mensagem em caso de erro
        } finally {
            setEnviando(false); // Desativa o estado de envio
        }
    }

    // Prepara o formulário com os dados do usuário para edição
    function editar(usuario: Usuario) {
        setIdEmEdicao(usuario.idUsuario); // Define qual usuário está em edição
        setFormulario({ nome: usuario.nome, email: usuario.email, idTipoUsuario: usuario.idTipoUsuario || "", senha: "" }); // Preenche o formulário
        setMensagem("Informe uma nova senha para concluir a edição."); // Mostra mensagem orientativa
        window.scrollTo({ top: 0, behavior: "smooth" }); // Rola a página para o topo suavemente
    }

    // Exclui o usuário selecionado no modal
    async function excluir() {
        if (!selecionado) return; // Se não houver selecionado, ignora
        setEnviando(true); // Ativa estado de envio
        try {
            await usuarioService.excluir(selecionado.idUsuario); // Deleta via API
            if (idEmEdicao === selecionado.idUsuario) limpar(); // Se deletar o que estava editando, limpa a tela
            setSelecionado(null); // Fecha o modal
            await carregar(); // Recarrega a lista atualizada
        } catch (error) {
            setMensagem(getApiErrorMessage(error)); // Exibe mensagem se falhar
            setSelecionado(null);
        } finally {
            setEnviando(false); // Desativa estado de envio
        }
    }

    return (
        <>
            {/* Renderiza o Cabeçalho */}
            <Header />
            <main className="cadastro-usuario-page">
                <section className="cadastro-usuario-formulario-section">
                    <h1 className="section-title">Administração de usuários</h1>
                    <div className="cadastro-usuario-conteudo">
                        <img src="" alt="Ilustração da administração de usuários" />
                        
                        {/* Formulário com evento de submissão ligado à função salvar */}
                        <form className="cadasrto-usuario-formulario" onSubmit={salvar}>
                            {/* Altera o título de acordo com a ação (Edição ou Cadastro) */}
                            <h2>{idEmEdicao ? "Editar Usuário" : "Cadastrar usuário"}</h2>
                            
                            {/* Campo para o Nome */}
                            <label className="visually-hidden" htmlFor="usuario-nome">Nome</label>
                            <Campo id="usuario-nome" name="nome" type="text" value={formulario.nome} onChange={atualizar} placeholder="Nome" maxLength={100} required />
                            
                            {/* Campo para o E-mail */}
                            <label className="visually-hidden" htmlFor="usuario-email">E-mail</label>
                            <Campo id="usuario-email" name="email" type="email" value={formulario.email} onChange={atualizar} placeholder="E-mail" required />
                            
                            <div className="cadastro-usuario-grid">
                                <div>
                                    <label className="visually-hidden" htmlFor="usuario-tipo">Tipo de usuário</label>
                                    {/* Select listando os tipos vindo do banco */}
                                    <select name="idTipoUsuario" id="usuario-tipo" value={formulario.idTipoUsuario} onChange={atualizar} required>
                                        {tipos.map((tipo) => <option value={tipo.idTipoUsuario} key={tipo.idTipoUsuario}>{tipo.titulo}</option>)}
                                    </select>
                                </div>
                                <div>
                                    <label className="visually-hidden" htmlFor="usuario-senha">Senha</label>
                                    {/* Campo para a Senha */}
                                    <Campo id="usuario-senha" name="senha" type="password" value={formulario.senha} onChange={atualizar} placeholder={idEmEdicao ? "Nova Senha" : "Senha"} minLength={8} maxLength={60} required />
                                </div>

                                {/* Botão principal do formulário */}
                                <Botao texto={enviando ? "Salvando..." : idEmEdicao ? "Salvar" : "Cadastrar"} type="submit" disabled={enviando} />
                                
                                {/* Exibe o botão de cancelar apenas se estiver editando */}
                                {idEmEdicao ? <button className="cadastro-usuario-cancelar" type="button" onClick={limpar}>Cancelar Edição</button> : null}
                                
                                {/* Exibe mensagens de feedback se houverem */}
                                {mensagem ? <p className="cadastro-usuario-mensagem" role="alert">{mensagem}</p> : null}
                            </div>
                        </form>
                    </div>
                </section>

                {/* Componente que exibe a tabela/lista de usuários */}
                <ListaUsuarios 
                    usuarios={usuarios} 
                    idEmEdicao={idEmEdicao} 
                    carregando={carregando} 
                    onEditar={editar} 
                    onExcluir={setSelecionado}
                />
                
                {/* Modal de confirmação antes de excluir */}
                <ModalConfirmacao 
                    show={Boolean(selecionado)} 
                    titulo="Excluir usuário" 
                    mensagem={selecionado ? `Deseja excluir "${selecionado.nome}"?` : ""}
                    textoConfirmar={enviando ? "Excluindo..." : "Excluir"}
                    textoCancelar="Cancelar"
                    onClose={() => setSelecionado(null)}
                    onConfirm={() => void excluir()}
                />
            </main>
            {/* Renderiza o Rodapé */}
            <Footer />
        </>
    );
}

// Exporta o componente Usuarios para ser utilizado em outros arquivos
export default Usuarios;