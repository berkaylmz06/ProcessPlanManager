using System;
using System.Collections.Generic;
using System.ComponentModel;

public class SiralabilirBindingList<T> : BindingList<T>
{
    private bool siraliMi;
    private ListSortDirection siralamaYonu;
    private PropertyDescriptor siralamaOzelligi;

    public SiralabilirBindingList() : base() { }
    public SiralabilirBindingList(IList<T> list) : base(list) { }

    protected override bool SupportsSortingCore => true;
    protected override bool IsSortedCore => siraliMi;
    protected override ListSortDirection SortDirectionCore => siralamaYonu;
    protected override PropertyDescriptor SortPropertyCore => siralamaOzelligi;

    protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
    {
        var itemsList = (List<T>)Items;
        var comparer = new OzellikKarsilastirici<T>(prop, direction);
        itemsList.Sort(comparer);

        siraliMi = true;
        siralamaYonu = direction;
        siralamaOzelligi = prop;

        ResetBindings();
    }

    protected override void RemoveSortCore()
    {
        siraliMi = false;
    }
}

public class OzellikKarsilastirici<T> : IComparer<T>
{
    private PropertyDescriptor ozellik;
    private ListSortDirection yon;

    public OzellikKarsilastirici(PropertyDescriptor ozellik, ListSortDirection yon)
    {
        this.ozellik = ozellik;
        this.yon = yon;
    }

    public int Compare(T x, T y)
    {
        var xValue = ozellik.GetValue(x);
        var yValue = ozellik.GetValue(y);

        int sonuc = Comparer<object>.Default.Compare(xValue, yValue);

        return yon == ListSortDirection.Ascending ? sonuc : -sonuc;
    }
}
