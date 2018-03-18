import * as React from 'react';
import { TextInputWithButtonDropdown } from '../Controls/TextInputWithButtonDropdown';
import * as Api from '../../api';

interface AddGameFormState {
    winner: string;
    loser: string;
    submitDisabled: boolean;
    players: string[];
}

export class AddGameForm extends React.Component<{}, AddGameFormState> {
    timerId: number;

    constructor() {
        super();
        this.state = { winner: '', loser: '', submitDisabled: false, players: [] };
        this.onSubmit = this.onSubmit.bind(this);
        this.onClearClicked = this.onClearClicked.bind(this);
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
            <div className="btn-group" role="group">
                <button type="submit" className="btn btn-default" disabled={this.state.submitDisabled}>Submit</button>
                <button type="button" className="btn btn-default" onClick={this.onClearClicked}>Clear</button>
            </div>
        </form>;
    }

    onSubmit(e: React.FormEvent<HTMLFormElement>) {
        e.preventDefault();

        this.setState({ submitDisabled: true });

        // POST game result and then clear state
        Api.postGame({
            winner: this.state.winner,
            loser: this.state.loser
        })
            .then(success => this.setState({
                winner: '', loser: '', submitDisabled: false
            }));
    }

    onClearClicked(e: React.MouseEvent<HTMLButtonElement>) {
        e.preventDefault();

        this.setState({
            winner: '', loser: ''
        });
    }

    onWinnerChange(winner: string) {
        this.setState({ winner: winner });
    }

    onWinnerSelected(winner: string) {
        this.setState({ winner: winner });
    }

    onLoserChange(loser: string) {
        this.setState({ loser: loser });
    }

    onLoserSelected(loser: string) {
        this.setState({ loser: loser });
    }

    fetchPlayers() {
        Api.getPlayers()
            .then(players => {
                this.setState({
                    players: players
                });
            })
    }

    componentWillMount() {
        this.fetchPlayers();
    }

    componentDidMount() {
        this.timerId = setInterval(() => this.fetchPlayers(), 2000);
    }

    componentWillUnmount() {
        clearInterval(this.timerId);
    }
}
