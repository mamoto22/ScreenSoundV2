﻿using Microsoft.AspNetCore.Mvc;
using ScreenSound.API.Request;
using ScreenSound.API.Response;
using ScreenSound.Banco;
using ScreenSound.Modelos;

namespace ScreenSound.API.EndPoints
{
    public static class ArtistasExtensions
    {
        private static ICollection<ArtistaResponse> EntityListToResponseList(IEnumerable<Artista> listaDeArtistas)
        {
            return listaDeArtistas.Select(a => EntityToResponse(a)).ToList();
        }

        private static ArtistaResponse EntityToResponse(Artista artista)
        {
            return new ArtistaResponse(artista.Id, artista.Nome, artista.Bio, artista.FotoPerfil);
        }
        public static void AddEndPointsArtistas(this WebApplication app) 
        {
            app.MapGet("/Artistas", ([FromServices] DAL<Artista> dal) =>
            {
                return Results.Ok(dal.Listar());
            });

            #region EndPoint Artistas
            app.MapGet("/Artistas/{nome}", ([FromServices] DAL<Artista> dal, string nome) =>
            {

                var artista = dal.RecuperarPor(a => a.Nome.ToUpper().Equals(nome.ToUpper()));

                if (artista is null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(artista);
            });

            app.MapPost("/Artistas", ([FromServices] DAL<Artista> dal, [FromBody] ArtistaRequest artistaRequest) => {

                var artista = new Artista(artistaRequest.nome, artistaRequest.bio);
                dal.Adicionar(artista);
                return Results.Ok();

            });

            app.MapDelete("/Artistas/{id}", ([FromServices] DAL<Artista> dal, int id) => {

                var artista = dal.RecuperarPor(a => a.Id == id);

                if (artista is null)
                {
                    return Results.NotFound();
                }

                dal.Deletar(artista);
                return Results.NoContent();

            });

            app.MapPut("/Artistas", ([FromServices] DAL<Artista> dal, [FromBody] Artista artista) => {
                var artistaAAtualizar = dal.RecuperarPor(a => a.Id == artista.Id);
                if (artistaAAtualizar is null)
                {
                    return Results.NotFound();
                }

                artistaAAtualizar.Nome = artista.Nome;
                artistaAAtualizar.Bio = artista.Bio;
                artistaAAtualizar.FotoPerfil = artista.FotoPerfil;

                dal.Atualizar(artistaAAtualizar);
                return Results.Ok();

            });

            
            #endregion

        }

    }
}
