import type { TipoEvento, Usuario } from "../../types/api";
import "./ListaTipoEvento.css"

import lapis from "../../assets/icons/lapis.svg";
import lixeira from  "../../assets/icons/lixeira.svg";

interface ListaTipoEventoProps {
    tipoEvento: TipoEvento[];
    idEmEdicao: string | null;
    carregando?: boolean;
    onEditar: (tipoEvento: TipoEvento) => void;
    onExcluir: (tipoEvento: TipoEvento) => void;
}

function ListaTipoEvento({ tipoEvento, idEmEdicao, carregando, onEditar, onExcluir }: ListaTipoEventoProps) {
    return (
        <section className="lista-tipos lista-tipoEvento">
            <div className="lista-tipos-conteudo">
                <h2>Tipos de Eventos Cadastrados</h2>
                <div className="lista-tipos-tabela-wrapper">
                    <table className="lista-tipoEvento-tabela">
                        <thead>
                            <tr>
                                <th>Título</th>
                                <th>Editar</th>
                                <th>Excluir</th>
                            </tr>
                        </thead>
                        <tbody>
                            {carregando
                                ? <tr><td colSpan={5} className="lista-tipos-tabela-vazia">Carregando...</td></tr>
                                : null
                            }
                            {!carregando && tipoEvento.length === 0
                                ? <tr><td colSpan={5} className="lista-tipos-tabela-vazia">Nenhum tipo cadastrado</td></tr>
                                : null
                            }
                            {!carregando && tipoEvento.map((tipo) => (
                                <tr className={idEmEdicao === tipo.idTipoEvento ? "lista-tipos-linha-ativa" : ""} key={tipo.idTipoEvento}>
                                    <td>{tipo.titulo}</td>
                                    <td><button className="lista-tipos-acao" type="button" aria-label={`Editar ${tipo.titulo}`} onClick={() => onEditar(tipo)}><img src={lapis} alt="icone de editar"/></button></td>
                                    <td><button className="lista-tipos-acao" type="button" aria-label={`Excluir ${tipo.titulo}`} onClick={() => onExcluir(tipo)}><img src={lixeira} alt="icone de excluir"/></button></td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>

            </div>
        </section>
    )

}

export default ListaTipoEvento;