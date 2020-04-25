import { Evento } from './../_models/Evento';
import { Component, OnInit, Injectable, TemplateRef } from '@angular/core';
import { EventoService } from '../_services/evento.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css'],
  providers: [EventoService]
})
export class EventosComponent implements OnInit {
    eventos: Evento[];
    imagemLargura = 50;
    imagemMargem =  2;
    mostraImagem = false;
    eventosFiltrados: Evento[];
    modalRef: BsModalRef;

    constructor(private eventoService: EventoService, private modalService: BsModalService) { }

    _filtroLista: string;
    get filtroLista(): string {
      return this._filtroLista;
    }
    set filtroLista(value: string) {
      this._filtroLista =  value;
      this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(this.filtroLista) : this.eventos;
    }

    openModal(template: TemplateRef<any>) {
       this.modalRef = this.modalService.show(template);
    }

  ngOnInit() {
    this.getEventos();
  }

  getEventos() {
    this.eventoService.getAllEventos().subscribe(
      (_eventos: Evento[]) => {
      this.eventos = _eventos;
      this.eventosFiltrados = _eventos;
      console.log(_eventos);
    }, error => {
      console.log(error);
    }
    );
  }

  alternarImagem() {
    this.mostraImagem = !this.mostraImagem;
  }

  filtrarEventos(filtro: string): Evento[] {
    filtro = filtro.toLocaleLowerCase();
    return this.eventos.filter(x => x.tema.toLocaleLowerCase().indexOf(filtro) > -1);
  }
}
