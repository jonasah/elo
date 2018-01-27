export namespace Models {
    export interface Rating {
        id: number;
        rank: number;
        player: string;
        rating: number;
        gamesPlayed: number;
        wins: number;
        losses: number;
        pct: number;
    }

    export interface ExpectedScore {
        opponent: string;
        score: number;
    }

    export interface Head2HeadRecord {
        opponent: string;
        gamesPlayed: number;
        wins: number;
        losses: number;
        pct: number;
    }

    export interface Game {
        id: number;
        winner: string;
        loser: string;
        date: string;
    }

    export interface GameResult {
        winner: string;
        loser: string;
    }
}

/* GET methods */

export function getRatings(): Promise<Models.Rating[]> {
    return get('ratings');
}

export function getPlayers(): Promise<string[]> {
    return get('players');
}

export function getLatestGames(numGames: number, player?: string): Promise<Models.Game[]> {
    var relativeUrl = 'games';

    if (player !== undefined) {
        relativeUrl += '/' + player;
    }

    relativeUrl += '?page=1&pageSize=' + numGames;

    return get(relativeUrl);
}

export function getHead2HeadRecords(player: string): Promise<Models.Head2HeadRecord[]> {
    return get('playerstats/' + player + '/h2h');
}

export function getExpectedScores(player: string): Promise<Models.ExpectedScore[]> {
    return get('playerstats/' + player + '/expectedscores');
}

/* POST methods */

export function postGame(gameResult: Models.GameResult): Promise<boolean> {
    return post('game', JSON.stringify(gameResult));
}

/* DELETE methods */
export function deleteGame(id: number): Promise<boolean> {
    return _delete(`game/${id}`);
}

/* Request methods */

function get<T>(relativeUrl: string): Promise<T> {
    return request(relativeUrl);
}

function post<T>(relativeUrl: string, body?: any): Promise<T> {
    return request(relativeUrl, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: body
    });
}

function _delete<T>(relativeUrl: string): Promise<T> {
    return request(relativeUrl, {
        method: 'DELETE'
    });
}

const baseUrl = 'api/elo/';

function request<T>(relativeUrl: string, init?: RequestInit): Promise<T> {
    return fetch(baseUrl + relativeUrl, init)
        .then(response => response.json() as Promise<T>);
}
