import * as React from 'react'

interface TextInputWithDropdownProps {
    name: string;
    value: string;
    items: string[];
    onValueChange: any;
    onItemSelected: any;
}

export class TextInputWithButtonDropdown extends React.Component<TextInputWithDropdownProps, {}> {
    constructor() {
        super();
        this.onInputChange = this.onInputChange.bind(this);
        this.onItemClick = this.onItemClick.bind(this);
    }

    public render() {
        return <div className="form-group">
            <label htmlFor="input" className="sr-only">{this.props.name}</label>
            <div className="input-group">
                <input type="text" className="form-control" placeholder={this.props.name} id="input" value={this.props.value} onChange={this.onInputChange} />
                <div className="input-group-btn">
                    <button type="button" className="btn btn-default dropdown-toggle" data-toggle="dropdown">
                        Select <span className="caret" />
                    </button>
                    <ul className="dropdown-menu dropdown-menu-right">
                        {this.props.items.map(item =>
                            <li key={item}><a href="#" onClick={this.onItemClick}>{item}</a></li>
                        )}
                    </ul>
                </div>
            </div>
        </div>;
    }

    onInputChange(e: React.ChangeEvent<HTMLInputElement>) {
        this.props.onValueChange(e);
    }

    onItemClick(e: React.MouseEvent<HTMLAnchorElement>) {
        this.props.onItemSelected(e);
    }
}