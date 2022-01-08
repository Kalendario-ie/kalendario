export interface TreeViewItem {
    id: number | string;
    name: string;
    children?: TreeViewItem[];
}
