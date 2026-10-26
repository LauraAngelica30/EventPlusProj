import { useCallback, useEffect, useState, type ChangeEvent, type FormEvent } from "react";
import Header from "../../components/header/Header";
import type { TipoEvento } from "../../types/api";
import "./TipoEvento.css"
import Campo from "../../components/campo/Campo";
import { tipoEventoService } from "../../Services/eventPlusservice";
import { getApiErrorMessage } from "../../Services/api";
import Botao from "../../components/botao/Botao";
import ListaTipoEvento from "../../components/listaTipoEvento/ListaTipoEvento";
import ModalConfirmacao from "../../components/modalConfirmacao/ModalConfirmacao";

interface FormularioTipoEvento { titulo: string; idTipoEvento: string; }

const inicial: FormularioTipoEvento = { titulo: "", idTipoEvento: "" }

function TipoEventoPage() {
    const [formulario, setFormulario] = useState(inicial);
    const [tipos, setTipos] = useState<TipoEvento[]>([]);
    const [idEmEdicao, setIdEmEdicao] = useState<string | null>(null);
    const [selecionado, setSelecionado] = useState<TipoEvento | null>(null);
    const [carregando, setCarregando] = useState(true);
    const [enviando, setEnviando] = useState(false);
    const [mensagem, setMensagem] = useState("")

    const carregar = useCallback(async () => {
        setCarregando(true);
        setMensagem("")
        try {
            const [tiposApi] = await Promise.all([tipoEventoService.listar()]);
            setTipos(tiposApi)
            setFormulario((value) => ({ ...value, idTipoEvento: value.idTipoEvento || tiposApi[0].idTipoEvento || "" }));
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
        setFormulario({ ...inicial, idTipoEvento: tipos[0]?.idTipoEvento || "" });
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
                idTipoEvento: formulario.idTipoEvento || null
            };

            if (idEmEdicao) await tipoEventoService.atualizar(idEmEdicao, payload.titulo);
            else await tipoEventoService.cadastrar(payload.titulo);
            limpar();
            await carregar();
        } catch (error) {
            setMensagem(getApiErrorMessage(error));
        } finally {
            setEnviando(false);
        }
    }

    function editar(tipoEvento: TipoEvento) {
        setIdEmEdicao(tipoEvento.idTipoEvento)
        setFormulario({ titulo: tipoEvento.titulo, idTipoEvento: tipoEvento.idTipoEvento });
        setMensagem("Informe um novo título para concluir a edição.");
        window.scrollTo({ top: 0, behavior: "smooth" })
    }

    async function excluir() {
        if (!selecionado) return;
        setEnviando(true);
        try {
            await tipoEventoService.excluir(selecionado.idTipoEvento);
            if (idEmEdicao === selecionado.idTipoEvento) limpar();
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
            <main className="cadastro-tipoEvento-page">
                <section className="cadastro-tipoEvento-formulario-section">
                    <h1 className="section-title">Administração de TipoEvento</h1>
                    <div className="cadastro-tipoEvento-conteudo">
                        <img src={null} alt="Ilustração da administração de tipos de eventos" />
                        <form className="cadastro-tipoEvento-formulario" onSubmit={salvar}>
                            <h2>{idEmEdicao ? "Editar tipo de evento" : "Cadastrar tipo de evento"}</h2>
                            <label className="visually-hidden" htmlFor="tipoEvento-titulo">Título</label>
                            <Campo id="tipoEvento-titulo" name="titulo" type="text" value={formulario.titulo} onChange={atualizar} placeholder="Título" maxLength={100} required />
                            <div className="cadastro-tipoEvento-grid">
                                <Botao texto={enviando ? "Salvando..." : idEmEdicao ? "Salvar" : "Cadastrar"} type="submit" disabled={enviando} />
                                {idEmEdicao ? <button className="cadastro-tipoEvento-cancelar" type="button" onClick={limpar}>Cancelar Edição</button> : null}
                                {mensagem ? <p className="cadastro-tipoEvento-mensagem" role="alert">{mensagem}</p> : null}
                            </div>
                        </form>
                    </div>
                </section>
                <ListaTipoEvento
                    tipoEvento={tipos}
                    idEmEdicao={idEmEdicao}
                    carregando={carregando}
                    onEditar={editar}
                    onExcluir={setSelecionado}
                />
                <ModalConfirmacao
                    show={Boolean(selecionado)}
                    titulo="Excluir tipo evento"
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

export default TipoEventoPage;