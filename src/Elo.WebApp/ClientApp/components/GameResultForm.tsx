import * as React from 'react';

interface GameResultFormState {
    winner: string;
    loser: string;
}

export class GameResultForm extends React.Component<{}, GameResultFormState> {
    constructor() {
        super();
        this.state = { winner: '', loser: '' };
        this.onSubmit = this.onSubmit.bind(this);
        this.onWinnerChange = this.onWinnerChange.bind(this);
        this.onLoserChange = this.onLoserChange.bind(this);
    }

    onSubmit(e: React.FormEvent<HTMLFormElement>) {
        e.preventDefault();

        // POST game result and then clear state
        fetch('api/elo/gameresults', {
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
            winner: e.target.value
        });
    }

    onLoserChange(e: React.ChangeEvent<HTMLInputElement>) {
        this.setState({
            loser: e.target.value
        });
    }

    public render() {
        return <form onSubmit={this.onSubmit}>
            <label>
                Winner: 
                <input type="text" name="winner" value={this.state.winner} onChange={this.onWinnerChange}/>
            </label>
            <label>
                Loser:
                <input type="text" name="loser" value={this.state.loser} onChange={this.onLoserChange} />
            </label>
            <input type="submit" value="Submit"/>
        </form>;
    }
}
