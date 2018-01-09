import * as React from 'react';
import { TextInputWithButtonDropdown } from '../Controls/TextInputWithButtonDropdown';

interface AddGameFormState {
    winner: string;
    loser: string;

    players: string[];
}

export class AddGameForm extends React.Component<{}, AddGameFormState> {
    timerId: number;

    constructor() {
        super();
        this.state = { winner: '', loser: '', players: [] };
        this.onSubmit = this.onSubmit.bind(this);
        this.onWinnerChange = this.onWinnerChange.bind(this);
        this.onWinnerSelected = this.onWinnerSelected.bind(this);
        this.onLoserChange = this.onLoserChange.bind(this);
        this.onLoserSelected = this.onLoserSelected.bind(this);
    }

    public render() {
        return <form onSubmit={this.onSubmit} className="form-inline">
            <TextInputWithButtonDropdown
                name="Winner"
                value={this.state.winner}
                items={this.state.players}
                onValueChange={this.onWinnerChange}
                onItemSelected={this.onWinnerSelected}
            />
            <TextInputWithButtonDropdown
                name="Loser"
                value={this.state.loser}
                items={this.state.players}
                onValueChange={this.onLoserChange}
                onItemSelected={this.onLoserSelected}
            />
            <button type="submit" className="btn btn-default">Submit</button>
        </form>;
    }

    onSubmit(e: React.FormEvent<HTMLFormElement>) {
        e.preventDefault();

        // POST game result and then clear state
        fetch('api/elo/game', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(this.state)
        })
            .then(response => this.setState({
                winner: '', loser: ''
            }));
    }

    onWinnerChange(e: React.ChangeEvent<HTMLInputElement>) {
        this.setState({
            winner: e.currentTarget.value
        });
    }

    onWinnerSelected(e: React.MouseEvent<HTMLAnchorElement>) {
        this.setState({
            winner: e.currentTarget.innerText
        });
    }

    onLoserChange(e: React.ChangeEvent<HTMLInputElement>) {
        this.setState({
            loser: e.currentTarget.value
        });
    }

    onLoserSelected(e: React.MouseEvent<HTMLAnchorElement>) {
        this.setState({
            loser: e.currentTarget.innerText
        });
    }

    fetchPlayers() {
        fetch('api/elo/players')
            .then(response => response.json() as Promise<string[]>)
            .then(players => {
                this.setState({
                    players: players
                });
            })
    }

    componentDidMount() {
        this.timerId = setInterval(() => this.fetchPlayers(), 2000);
    }

    componentWillUnmount() {
        clearInterval(this.timerId);
    }
}
