import { useCallback, useEffect, useState, type ChangeEvent, type FormEvent } from "react";
import Header from "../../components/header/Header";
import type { TipoEvento, TipoUsuario } from "../../types/api";
import "./TipoUsuario.css"
import Campo from "../../components/campo/Campo";
import { tipoEventoService, tipoUsuarioService } from "../../Services/eventPlusservice";
import { getApiErrorMessage } from "../../Services/api";
import Botao from "../../components/botao/Botao";
import ModalConfirmacao from "../../components/modalConfirmacao/ModalConfirmacao";
import ListaTipoUsuario from "../../components/listaTipoUsuario/ListaTipoUsuario";

interface FormularioTipoUsuario { titulo: string; idTipoUsuario: string; }

const inicial: FormularioTipoUsuario = { titulo: "", idTipoUsuario: "" }

function TipoUsuarioPage() {
    const [formulario, setFormulario] = useState(inicial);
    const [tipos, setTipos] = useState<TipoUsuario[]>([]);
    const [idEmEdicao, setIdEmEdicao] = useState<string | null>(null);
    const [selecionado, setSelecionado] = useState<TipoUsuario | null>(null);
    const [carregando, setCarregando] = useState(true);
    const [enviando, setEnviando] = useState(false);
    const [mensagem, setMensagem] = useState("")

    const carregar = useCallback(async () => {
        setCarregando(true);
        setMensagem("")
        try {
            const [tiposApi] = await Promise.all([tipoUsuarioService.listar()]);
            setTipos(tiposApi)
            setFormulario((value) => ({ ...value, idTipoUsuario: value.idTipoUsuario || tiposApi[0].idTipoUsuario || "" }));
        } catch (error) {
            setMensagem(getApiErrorMessage(error));
        } finally {
            setCarregando(false);
        }
    }, [])

    useEffect(() => {
        const timeout = window.setTimeout(() => void carregar(), 0);
        return () => window.clearTimeout(timeout);
    }, [carregar]);

    function atualizar(event: ChangeEvent<HTMLInputElement | HTMLSelectElement>) {
        setFormulario((value) => ({ ...value, [event.target.name]: event.target.value }))
    }

    function limpar() {
        setFormulario({ ...inicial, idTipoUsuario: tipos[0]?.idTipoUsuario || "" });
        setIdEmEdicao(null);
        setMensagem("");
    }

    async function salvar(event: FormEvent<HTMLFormElement>) {
        event.preventDefault()
        setEnviando(true);
        setMensagem("")
        try {
            const payload = {
                ...formulario,
                titulo: formulario.titulo.trim(),
                idTipoEvento: formulario.idTipoUsuario || null
            };

            if (idEmEdicao) await tipoUsuarioService.atualizar(idEmEdicao, payload.titulo);
            else await tipoUsuarioService.cadastrar(payload.titulo);
            limpar();
            await carregar();
        } catch (error) {
            setMensagem(getApiErrorMessage(error));
        } finally {
            setEnviando(false);
        }
    }

    function editar(tipoUsuario : TipoUsuario) {
        setIdEmEdicao(tipoUsuario.idTipoUsuario)
        setFormulario({ titulo: tipoUsuario.titulo, idTipoUsuario: tipoUsuario.idTipoUsuario });
        setMensagem("Informe um novo título para concluir a edição.");
        window.scrollTo({ top: 0, behavior: "smooth" })
    }

    async function excluir() {
        if (!selecionado) return;
        setEnviando(true);
        try {
            await tipoUsuarioService.excluir(selecionado.idTipoUsuario);
            if (idEmEdicao === selecionado.idTipoUsuario) limpar();
            setSelecionado(null);
            await carregar();
        } catch (error) {
            setMensagem(getApiErrorMessage(error));
            setSelecionado(null);
        } finally {
            setEnviando(false);
        }
    }

    return (
        <>
            <Header />
            <main className="cadastro-tipoUsuario-page">
                <section className="cadastro-tipoUsuario-formulario-section">
                    <h1 className="section-title">Administração de TipoUsuario</h1>
                    <div className="cadastro-tipoUsuario-conteudo">
                        <img src={null} alt="Ilustração da administração de tipos de usuarios" />
                        <form className="cadastro-tipoUsuario-formulario" onSubmit={salvar}>
                            <h2>{idEmEdicao ? "Editar tipo de usuario" : "Cadastrar tipo de usuario"}</h2>
                            <label className="visually-hidden" htmlFor="tipoUsuario-titulo">Título</label>
                            <Campo id="tipoUsuario-titulo" name="titulo" type="text" value={formulario.titulo} onChange={atualizar} placeholder="Título" maxLength={100} required />
                            <div className="cadastro-tipoUsuario-grid">
                                <Botao texto={enviando ? "Salvando..." : idEmEdicao ? "Salvar" : "Cadastrar"} type="submit" disabled={enviando} />
                                {idEmEdicao ? <button className="cadastro-tipoUsuario-cancelar" type="button" onClick={limpar}>Cancelar Edição</button> : null}
                                {mensagem ? <p className="cadastro-tipoUsuario-mensagem" role="alert">{mensagem}</p> : null}
                            </div>
                        </form>
                    </div>
                </section>
                <ListaTipoUsuario
                    tipoUsuario={tipos}
                    idEmEdicao={idEmEdicao}
                    carregando={carregando}
                    onEditar={editar}
                    onExcluir={setSelecionado}
                />
                <ModalConfirmacao
                    show={Boolean(selecionado)}
                    titulo="Excluir tipo usuario"
                    mensagem={selecionado ? `Deseja excluir "${selecionado.titulo}"?` : ""}
                    textoConfirmar={enviando ? "Excluindo..." : "Excluir"}
                    textoCancelar="Cancelar"
                    onClose={() => setSelecionado(null)}
                    onConfirm={() => void excluir()}
                />
            </main>
        </>
    );
}

export default TipoUsuarioPage;