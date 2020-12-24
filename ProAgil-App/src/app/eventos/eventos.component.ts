import { Evento } from './../_models/Evento';
import { Component, OnInit, Injectable, TemplateRef } from '@angular/core';
import { EventoService } from '../_services/evento.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { defineLocale } from 'ngx-bootstrap/chronos';
import { ptBrLocale } from 'ngx-bootstrap/locale';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';

defineLocale('pt-br', ptBrLocale);

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css'],
  providers: [EventoService]
})
export class EventosComponent implements OnInit {
    eventos: Evento[];
    evento: Evento;
    imagemLargura = 50;
    imagemMargem =  2;
    mostraImagem = false;
    eventosFiltrados: Evento[];
    registerForm: FormGroup;
    modoSalvar = 'post';
    bodyDeletarEvento = '';
    Titulo = 'Eventos';
    Imagem: File;
    fileNameToUpdate: string;
    dataAtual: string;

    constructor(
      private eventoService: EventoService,
      private modalService: BsModalService,
      private fb: FormBuilder,
      private localService: BsLocaleService
      )
      {
        this.localService.use('pt-br');
      }

    _filtroLista: string;
    get filtroLista(): string {
      return this._filtroLista;
    }
    set filtroLista(value: string) {
      this._filtroLista =  value;
      this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(this.filtroLista) : this.eventos;
    }

    openModal(template: any) {
      this.registerForm.reset();
       template.show();
       this.modoSalvar = 'post';
    }

  ngOnInit() {
    this.validation()
    this.getEventos();
  }

  getEventos() {
    this.eventoService.getAllEventos().subscribe(
      (_eventos: Evento[]) => {
      this.eventos = _eventos;
      this.eventosFiltrados = _eventos;
    }, error => {
      console.log(error);
    }
    );
  }

  openModalEventoId(id: number, template: any) {
    this.registerForm.reset();
    this.eventoService.getEventoById(id).subscribe(
      (evento: Evento) => {
          this.evento = evento;
          this.evento.imageURL = '';
          this.registerForm.patchValue(evento);
          this.fileNameToUpdate = evento.imageURL.toString();
          template.show();
          this.modoSalvar = 'put';
      },
      (e) => {
        console.log(e);
      }
    );
  }

  alternarImagem() {
    this.mostraImagem = !this.mostraImagem;
  }

  validation() {
    this.registerForm = this.fb.group({
      tema: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      local: ['', Validators.required],
      dataEvento: ['', Validators.required],
      qtdPessoas: ['', [Validators.required, Validators.max(120000)]],
      imageURL: ['', Validators.required],
      telefone: ['', [Validators.required, Validators.minLength(9), Validators.maxLength(9)]],
      email: ['', [Validators.required, Validators.email]]
    });
  }

  uploadImagem() {
    if (this.modoSalvar === 'post') {
      const nomeArquivo = this.evento.imageURL.split('\\', 3);
      this.evento.imageURL = nomeArquivo[2];
      this.eventoService.postUpload(this.Imagem, nomeArquivo[2]).subscribe(
        () => {
         this.dataAtual = new Date().getMilliseconds().toString();
         this.getEventos();
        }
      );
    } else {
       this.evento.imageURL = this.fileNameToUpdate;
      this.eventoService.postUpload(this.Imagem, this.fileNameToUpdate).subscribe(
        () => {
          this.dataAtual = new Date().getMilliseconds().toString();
          this.getEventos();
         }
      );
    }
  }

  salvarAlteracao(modal: any) {
    if (this.registerForm.valid) {
      if (this.modoSalvar === 'post') {
        this.evento = Object.assign({}, this.registerForm.value);

      this.uploadImagem();

        this.eventoService.postEvento(this.evento).subscribe(
          (novoEvento: Evento) => {
            console.log(novoEvento);
            modal.hide();
            this.getEventos();
          },
          (error) => {
            console.log(error);
          }
        );
      } else {
        this.evento = Object.assign({id: this.evento.id }, this.registerForm.value);

        this.uploadImagem();

        this.eventoService.putEvento(this.evento).subscribe(
          () => {
            modal.hide();
            this.getEventos();
          },
          (error) => {
            console.log(error);
          }
        );
      }
    }
  }

  confirmeDelete(modal: any) {
    this.eventoService.deleteEvento(this.evento.id).subscribe(
      () => {
        modal.hide();
        this.getEventos();
      },
      (e) => {
        console.log(e);
      }
    );
  }

  excluirEvento(evento: Evento, template: any)  {
    template.show();
    this.evento = evento;
    this.bodyDeletarEvento = `Tem certeza que deseja deletar o evento: ${ evento.tema } ?`;
  }

  filtrarEventos(filtro: string): Evento[] {
    filtro = filtro.toLocaleLowerCase();
    return this.eventos.filter(x => x.tema.toLocaleLowerCase().indexOf(filtro) > -1);
  }

  onFileChange(event) {
    const reader = new FileReader();

    if(event.target.files && event.target.files.length) {
      this.Imagem = event.target.files;
      console.log(this.Imagem);
    }
  }
}
