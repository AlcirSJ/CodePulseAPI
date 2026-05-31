# Handoff Frontend — Adaptar ao formato paginado da API

> Documento para ser passado a um agente especialista em frontend Angular.
> Contexto: o backend (CodePulseAPI / .NET 8) introduziu paginação nos endpoints de
> listagem. Isso é um **breaking change** — o frontend precisa se adaptar.

## Resumo do que mudou no backend

Os endpoints de listagem **deixaram de retornar um array cru** e agora retornam um
**envelope paginado** (`PagedResult<T>`).

Endpoints afetados:
- `GET /api/blogposts`
- `GET /api/categories`

Os demais endpoints (`GET /{id}`, `POST`, `PUT`, `DELETE`) **não mudaram**.

## Antes vs. Depois do contrato

### Antes (array cru)
```json
GET /api/blogposts

[
  { "id": "...", "title": "...", "categories": [...] },
  { "id": "...", "title": "...", "categories": [...] }
]
```

### Depois (envelope paginado)
```json
GET /api/blogposts?page=1&pageSize=20

{
  "items": [
    { "id": "...", "title": "...", "categories": [...] }
  ],
  "page": 1,
  "pageSize": 20,
  "totalItems": 137,
  "totalPages": 7
}
```

## Parâmetros de query (novos)

| Parâmetro  | Tipo | Default | Regras                                              |
|------------|------|---------|-----------------------------------------------------|
| `page`     | int  | 1       | Se < 1, o backend força 1                           |
| `pageSize` | int  | 20      | Se < 1 ou > 100, o backend força 20 (máximo 100)    |

Ordenação aplicada pelo backend (o frontend não controla por enquanto):
- **BlogPosts:** por `publishedDate` decrescente (mais recentes primeiro)
- **Categories:** por `name` ascendente (alfabética)

## Tarefas no frontend Angular

1. **Criar a interface do envelope** (model genérico reutilizável):
   ```typescript
   export interface PagedResult<T> {
     items: T[];
     page: number;
     pageSize: number;
     totalItems: number;
     totalPages: number;
   }
   ```

2. **Atualizar os services Angular** (`BlogPostService`, `CategoryService`):
   - Os métodos `getAllBlogPosts()` / `getAllCategories()` hoje retornam
     `Observable<BlogPost[]>` / `Observable<Category[]>`.
   - Passar a retornar `Observable<PagedResult<BlogPost>>` /
     `Observable<PagedResult<Category>>`.
   - Aceitar parâmetros `page` e `pageSize` e enviá-los via `HttpParams`.

3. **Atualizar os componentes que consomem esses services:**
   - Onde hoje se faz `this.blogPosts = response`, passar a usar
     `this.blogPosts = response.items`.
   - Guardar `totalItems` / `totalPages` / `page` para os controles de navegação.

4. **Adicionar UI de paginação:**
   - Controles de "próxima / anterior" ou numeração de páginas.
   - Sugestão: usar o paginator do Angular Material (`mat-paginator`) se o projeto
     já usa Material; caso contrário, controles simples com `page` / `totalPages`.

5. **Verificar usos diretos da lista** (filtros, contadores, `*ngFor`) que assumiam
   um array no nível raiz da resposta — todos precisam apontar para `.items`.

## Critério de pronto (Definition of Done)

- [ ] Listagem de blog posts carrega e navega entre páginas sem erro de tipo.
- [ ] Listagem de categorias idem.
- [ ] Contador de total de itens/páginas exibido corretamente.
- [ ] Nenhum lugar do código ainda trata a resposta de listagem como array cru.
- [ ] `pageSize` enviado pelo frontend respeita o limite de 100.

## Observações

- O backend faz "clamp" silencioso de valores inválidos (não retorna erro 400 para
  `page`/`pageSize` fora do range) — o frontend pode confiar que sempre recebe uma
  página válida de volta, mas deve evitar enviar `pageSize > 100` pois será ignorado.
- Não há endpoint de "todos sem paginação". Se algum fluxo precisar de todos os
  registros, terá que iterar as páginas.
