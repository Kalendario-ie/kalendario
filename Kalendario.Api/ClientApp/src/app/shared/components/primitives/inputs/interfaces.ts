import React from 'react';
import { TreeViewItem } from '../containers';

export interface KBaseInputProps extends React.InputHTMLAttributes<any> {

}


export interface SelectOption {
    id: number | string; //todo: fix here to string only.
    name: string;
}

export interface MultiSelectOption extends SelectOption, TreeViewItem {
}
