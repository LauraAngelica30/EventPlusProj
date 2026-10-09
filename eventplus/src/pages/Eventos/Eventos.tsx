import { useEffect, useState } from "react";
import { eventoService } from "../../Services/eventPlusservice";
import type { Evento } from "../../types/api";

import "./Eventos.css"

import Header from "../../components/header/Header";
import Footer from "../../components/footer/Footer";
import CardEvento from "../../components/cardEvento/CardEvento";

function Eventos() {
    const [eventos, setEventos] = useState<Evento[]>([])
    // const [data, setData] = useState<Date>(new Date())

    useEffect(() => {
        // const timerId = setInterval(() => {
        //     setData(new Date());
        // }, 1000)

        // return () => {
        //     clearInterval(timerId);
        // };

        async function carregar() {
            const dados = await eventoService.listar();

            setEventos(dados);
        }

        void carregar();
        
    }, [])

    return (
        <>
        <Header/>
        <main className="catalogoEventos">
            <h1>Catálogo de eventos</h1>
            {/* <p>{data.toLocaleString()}</p> */}
            <div className="cards">
                {eventos.map((evento) => (
                    <CardEvento
                    key={evento.idEvento}
                    id={evento.idEvento}
                    imagem={evento.imagemUrl}
                    categoria={evento.idTipoEventoNavigation?.titulo}
                    titulo={evento.nome}
                    descricao={evento.descricao}/>
                ))}
            </div>
        </main>
        <Footer/>
        </>
    )
}

export default Eventos;